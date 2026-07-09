using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Groups.GetList;

namespace WaterCalculator.Features.Groups.GetDictionary
{
    public class GetGroupDictionaryHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<GetGroupsHandler> logger)
    {
        public async Task<Result<List<GroupDictionaryOption>>> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await cache.GetOrSetAsync(
                    GroupCachePolicy.CollectionKey,
                    async ct =>
                    {
                        await using var context =
                            await dbContextFactory.CreateDbContextAsync(ct);

                        return await context.Groups
                            .OrderBy(g => g.Name)
                            .Select(g => new GroupDictionaryOption(g.Id, g.Name))
                            .ToListAsync(ct);
                    },
                    TimeSpan.FromMinutes(30),
                    [GroupCachePolicy.Tag],
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured fetching group dictionary - {Exception}", ex);
                return Errors.ApplicationError;
            }
        }
    }
}
