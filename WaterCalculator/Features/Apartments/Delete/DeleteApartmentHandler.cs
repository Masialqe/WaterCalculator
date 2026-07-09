using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Groups.GetList;

namespace WaterCalculator.Features.Apartments.Delete
{
    public class DeleteApartmentHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<GetGroupsHandler> logger)
    {
        public async Task<Result> HandleAsync(Guid apartmentId,
           CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var deletedRows = await context.Apartments
                    .Where(a => a.Id == apartmentId)
                    .ExecuteDeleteAsync();

                var result = deletedRows != 0
                    ? Result.Success()
                    : Errors.InvalidOperationError("Nie udało się usunąć zasobu.");

                if (result.IsFailure) return result;

                await cache.RemoveByTagAsync(ApartmentCachePolicy.Tag);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured deleting apartment - {ApartmentId} - {Exception}.",
                    apartmentId,
                    ex);
                return Errors.ApplicationError;
            }
        }
    }
}
