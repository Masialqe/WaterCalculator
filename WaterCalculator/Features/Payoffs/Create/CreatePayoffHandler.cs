using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Features.Payoffs.Create
{
    public sealed record CreatePayoffCommand(DateTime DateFrom, DateTime DateTo, Guid GroupId);
    public sealed class CreatePayoffHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<CreatePayoffHandler> logger)
    {
        public async Task<Result> HandleAsync(CreatePayoffCommand command, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                var groupPayoffs = await context.Payoffs
                    .Where(p => p.GroupId == command.GroupId)
                    .Select(p => p.Status)
                    .ToListAsync();

                if (groupPayoffs.Any(s => s != PayoffStatus.Settled)) 
                        return Errors.InvalidOperationError("Dla tej grupy istnieje aktywne rozliczenie.");

                if (!await context.Apartments.AnyAsync(a => a.GroupId == command.GroupId))
                    return Errors.InvalidOperationError("Grupa musi posiadać conajmniej jedno mieszkanie.");

                var isFirstPayoff = groupPayoffs.Count == 0;

                if(isFirstPayoff)
                {
                    var hasApartmentWithoutRead = await context.Apartments
                        .Where(a => a.GroupId == command.GroupId)
                        .AnyAsync(a => !a.Reads.Any(), cancellationToken);

                    if (hasApartmentWithoutRead)
                    {
                        return Errors.InvalidOperationError(
                            "Nie wszystkie mieszkania posiadają odczyt początkowy.");
                    }

                }

                var payoff = Payoff.Create(command.DateFrom, command.DateTo, command.GroupId);

                var payoffValidationResult = payoff.Validate();
                if (payoffValidationResult.IsFailure) return payoffValidationResult.Error;

                await context.AddAsync(payoff, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch(Exception ex)
            {
                logger.LogError("An error occured creating payoff for group - {GroupId} - {ErrorMessage} - {Exception}",
                    command.GroupId,
                    ex.Message,
                    ex);
                return Errors.ApplicationError;
            }
        }
    }
}
