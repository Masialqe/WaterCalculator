using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Groups.GetList;

namespace WaterCalculator.Features.Apartments.GetAsList
{
    public sealed class GetApartmentByIdHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<GetGroupsHandler> logger)
    {
        public async Task<Result<Apartment>> HandleAsync(Guid apartmentId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var apartment = await cache.GetOrSetAsync(
                    ApartmentCachePolicy.UniqueKey(apartmentId),
                    async ct =>
                    {
                        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                        var group = await context.Apartments
                            .AsNoTracking()
                            .FirstOrDefaultAsync(g => g.Id == apartmentId, cancellationToken);

                        return group;
                    },
                    TimeSpan.FromMinutes(5),
                    [ApartmentCachePolicy.Tag],
                    cancellationToken);

                if (apartment is null) return Errors.NotFoundError;

                return apartment;
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured fetching apartment - {apartmentId} - {ErrorMessage} - {Exception}",
                    apartmentId, ex.Message, ex);
                return Errors.ApplicationError;
            }
        }
    }
}
