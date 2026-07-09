using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Payoffs.Settle;

namespace WaterCalculator.Features.Payoffs.Summarize
{
    public sealed class SummarizePayoffHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<SummarizePayoffHandler> logger)
    {
        public async Task<Result> HandleAsync(Guid payoffId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                var payoff = await context.Payoffs
                    .Include(p => p.Reads)
                    .FirstOrDefaultAsync(p => p.Id == payoffId, cancellationToken);

                if (payoff is null) return Errors.NotFoundError;

                if (payoff.Status != PayoffStatus.Open)
                    return Errors.InvalidOperationError("Nie można ponownie podsumować rozliczenia.");

                var apartmentsFromGroupCount = await context.Apartments
                    .Where(a => a.GroupId == payoff.GroupId)
                    .CountAsync(cancellationToken);

                if (apartmentsFromGroupCount != payoff.ReadsCount)
                    return Errors.InvalidOperationError("Nie wszystkie mieszkania posiadają odczyty.");

                var totalMeterValue = payoff.Reads.Sum(r => r.Value);
                var totalConsuptionValue = payoff.Reads.Sum(r => r.ConsumptionFromLastRead);

                payoff.TotalMeterValue = totalMeterValue;
                payoff.TotalConsumptionValue = totalConsuptionValue;
                payoff.Status = PayoffStatus.Summarized;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured generating calculation of payoff - {PayoffId} - {ErrorMessage} - {Exception}.",
                    payoffId,
                    ex.Message,
                    ex);
                return Errors.ApplicationError;
            }
        }
    }
}
