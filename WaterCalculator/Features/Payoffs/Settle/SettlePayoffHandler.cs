using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Payoffs.Summarize;

namespace WaterCalculator.Features.Payoffs.Settle
{
    public sealed class SettlePayoffHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        InvoiceStateValidator invoiceStateValidator,
        PayoffSettlementCalculator settlementCalculator,
        ILogger<SummarizePayoffHandler> logger)
    {
        public async Task<Result> HandleAsync(
            Guid payoffId,
            CancellationToken cancellationToken = default)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            try
            {
                var payoff = await context.Payoffs
                    .Include(p => p.Reads)
                    .Include(p => p.Invoice)
                        .ThenInclude(i => i!.InvoiceItems)
                    .FirstOrDefaultAsync(p => p.Id == payoffId, cancellationToken);

                if (payoff is null)
                    return Errors.NotFoundError;

                if (await context.Settlements.AnyAsync(s => s.InvoiceId == payoff.InvoiceId))
                    return Errors.InvalidOperationError("Dla tej faktury zostało wprowadzone rozliczenie.");

                var invoiceStateValidationResult = invoiceStateValidator.Validate(payoff);
                if (invoiceStateValidationResult.IsFailure)
                    return invoiceStateValidationResult.Error;

                var settlementsResult = settlementCalculator.CalculateSettlements(payoff);
                if (settlementsResult.IsFailure)
                    return settlementsResult.Error;

                var settlements = settlementsResult.Value;

                await context.AddRangeAsync(settlements, cancellationToken);
                payoff.Status = PayoffStatus.Settled;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "An error occurred generating settlement for payoff {PayoffId}",
                    payoffId);

                return Errors.ApplicationError;
            }
        }
    }
}
