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
                    .FirstOrDefaultAsync(p => p.Id == payoffId, cancellationToken);

                if (payoff is null) return Errors.NotFoundError;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
