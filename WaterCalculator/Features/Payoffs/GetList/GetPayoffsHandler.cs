using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Abstractions;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Payoffs.Get;

namespace WaterCalculator.Features.Payoffs.GetList;

public sealed class GetPayoffsHandler(
    IDbContextFactory<DatabaseContext> dbContextFactory,
    IAppCache cache,
    ILogger<GetPayoffDetailsHandler> logger)
{
    public async Task<Result<PageResult<PayoffListItem>>> HandleAsync(GetPayoffsQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await cache.GetOrSetAsync(
                PayoffCachePolicy.PagedKey(query.Page, query.PageSize),
                async ct =>
                {
                    await using var context = await dbContextFactory.CreateDbContextAsync(ct);

                    var totalCount = await context.Payoffs.CountAsync(ct);

                    var items = await context.Payoffs
                        .Where(p => p.Status == PayoffStatus.Settled)
                        .OrderBy(p => p.CreatedAt)
                        .Skip((query.Page - 1) * query.PageSize)
                        .Take(query.PageSize)
                        .Select(p => new PayoffListItem(
                            p.Id,
                            p.Group.Name,
                            p.PeriodFrom,
                            p.PeriodTo,
                            p.TotalMeterValue,
                            p.TotalConsumptionValue))
                        .ToListAsync(ct);

                    var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

                    return new PageResult<PayoffListItem>(
                        items,
                        query.Page,
                        query.PageSize,
                        totalCount,
                        totalPages);
                },
                TimeSpan.FromMinutes(30),
                [PayoffCachePolicy.Tag],
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occured while getting payoffs list - {Exception}", 
                ex);
            return Errors.ApplicationError;
        }
    }
}