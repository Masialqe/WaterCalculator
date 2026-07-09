using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Abstractions;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Features.Groups.GetList
{
    public sealed class GetGroupsHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<GetGroupsHandler> logger)
    {
        public async Task<Result<PageResult<Group>>> HandleAsync(GetGroupsQuery query,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await cache.GetOrSetAsync(
                    GroupCachePolicy.PagedKey(query.Page, query.PageSize),
                    async ct =>
                    {
                        await using var context = await dbContextFactory.CreateDbContextAsync(ct);

                        var totalCount = await context.Groups.CountAsync(ct);

                        var items = await context.Groups
                            .AsNoTracking()
                            .OrderBy(g => g.Name)
                            .Skip((query.Page - 1) * query.PageSize)
                            .Take(query.PageSize)
                            .ToListAsync(ct);

                        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

                        return new PageResult<Group>(
                            items,
                            query.Page,
                            query.PageSize,
                            totalCount,
                            totalPages);
                    },
                    TimeSpan.FromMinutes(30),
                    [GroupCachePolicy.Tag],
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured fetching list of groups - {Message} - {Exception}",
                    ex.Message, ex);
                return Errors.ApplicationError;
            }
        }
    }
}
