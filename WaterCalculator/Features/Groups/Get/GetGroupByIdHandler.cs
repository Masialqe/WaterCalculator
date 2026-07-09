using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Groups.GetList;

namespace WaterCalculator.Features.Groups.Get
{
    public sealed class GetGroupByIdHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<GetGroupsHandler> logger)
    {
        public async Task<Result<Group>> HandleAsync(Guid groupId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var group = await cache.GetOrSetAsync(
                    GroupCachePolicy.UniqueKey(groupId),
                    async ct =>
                    {
                        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                        var group = await context.Groups
                            .AsNoTracking()
                            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);

                        return group;
                    },
                    TimeSpan.FromMinutes(5),
                    [GroupCachePolicy.Tag],
                    cancellationToken);

                if (group is null) return Errors.NotFoundError;

                return group;
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured fetching group - {GroupId} - {ErrorMessage} - {Exception}",
                    groupId, ex.Message, ex);
                return Errors.ApplicationError;
            }
        }
    }
}
