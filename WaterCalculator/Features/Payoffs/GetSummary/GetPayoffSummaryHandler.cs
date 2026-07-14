using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Payoffs.Get;

namespace WaterCalculator.Features.Payoffs.GetSummary
{
    public sealed class GetPayoffSummaryHandler(
        IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<GetPayoffDetailsHandler> logger)
    {
        public async Task<Result<PayoffSummary>> HandleAsync(Guid payoffId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var payoff = await context.Payoffs
                    .AsNoTracking()
                    .Include(p => p.Group)
                    .Include(p =>p.Invoice)
                    .FirstOrDefaultAsync(p => p.Id == payoffId, cancellationToken);

                if (payoff is null) return Errors.NotFoundError;

                return await cache.GetOrSetAsync(
                    PayoffCachePolicy.SummaryUniqueKey(payoffId),
                    async ct =>
                    {
                        var settlements = await context.Settlements
                            .Where(s => s.PayoffId == payoffId)
                            .Select(s => new PayoffSummaryItem(
                                s.ApartmentId,
                                s.Apartment.Name,
                                s.Consumption,
                                s.AmountToPay,
                                s.RealizationStatus))
                            .ToListAsync(ct);

                        return new PayoffSummary(
                            payoff.Group.Name,
                            settlements.Sum(s => s.AmountToPay),
                            settlements.Count,
                            payoff,
                            settlements);
                    },
                    TimeSpan.FromMinutes(30),
                    [PayoffCachePolicy.Tag],
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured while getting payoff - {PayoffId} details - {Exception}",
                    payoffId, ex);
                return Errors.ApplicationError;
            }
        }
    }
}
