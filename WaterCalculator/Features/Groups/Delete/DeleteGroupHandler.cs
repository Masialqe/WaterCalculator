using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Groups.GetList;

namespace WaterCalculator.Features.Groups.Delete
{
    public sealed class DeleteGroupHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<GetGroupsHandler> logger)
    {
        public async Task<Result> HandleAsync(Guid groupId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var deletedRows = await context.Groups
                    .Where(g => g.Id == groupId)
                    .ExecuteDeleteAsync();

                var result = deletedRows != 0
                    ? Result.Success()
                    : Errors.InvalidOperationError("Nie udało się usunąć zasobu.");

                if (result.IsFailure) return result;

                await cache.RemoveByTagAsync(GroupCachePolicy.Tag);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured deleting group - {GroupId} - {Exception}.",
                    groupId,
                    ex);
                return Errors.ApplicationError;
            }
        }
    }
}
