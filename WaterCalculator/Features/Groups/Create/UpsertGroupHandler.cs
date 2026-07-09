using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Features.Groups.Create
{
    public sealed class UpsertGroupHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<UpsertGroupHandler> logger)
    {
        public async Task<Result> HandleAsync(UpsertGroupCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                var result = command.GroupId == null || command.GroupId == Guid.Empty
                    ? await CreateGroupAsync(context, command, cancellationToken)
                    : await UpdateGroupAsync(context, command, cancellationToken);
     
                if (result.IsFailure) return result.Error;

                await context.SaveChangesAsync();
                await cache.RemoveByTagAsync(GroupCachePolicy.Tag);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError("An error ocurred adding new group - {GroupName} - {ErrorMessage} - {Error}",
                    command.GroupName, ex.Message, ex);
                return Errors.ApplicationError;
            }
        }

        private async Task<Result> CreateGroupAsync(DatabaseContext context, 
            UpsertGroupCommand command, CancellationToken cancellationToken = default)
        {
            if(await IsGroupExists(context, command.GroupName, null, cancellationToken))
                return Errors.AlreadyExistsError;

            var group = Group.Create(command.GroupName, command.GroupDetails);
            await context.AddAsync(group, cancellationToken);

            return Result.Success();
        }

        private async Task<Result> UpdateGroupAsync(DatabaseContext context,
            UpsertGroupCommand command, CancellationToken cancellationToken = default)
        {
            var existingState = await context.Groups
                .FirstOrDefaultAsync(x => x.Id == command.GroupId, cancellationToken);

            if (existingState == null) return Errors.NotFoundError;

            if(await IsGroupExists(context, command.GroupName, command.GroupId, cancellationToken))
                return Errors.AlreadyExistsError;

            existingState.Name = command.GroupName;
            existingState.Details = command.GroupDetails;

            return Result.Success();
        }

        private async Task<bool> IsGroupExists(
            DatabaseContext context,
            string groupName,
            Guid? excludedGroupId = null,
            CancellationToken cancellationToken = default)
            => await context.Groups.AnyAsync(x =>
                    x.Name == groupName &&
                    (excludedGroupId == null || x.Id != excludedGroupId),
                        cancellationToken);

    }
}
