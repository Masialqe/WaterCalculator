using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Abstractions;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Features.Apartments.GetAsList;

public sealed class GetApartmentsHandler(IDbContextFactory<DatabaseContext> dbContextFactory, 
    IAppCache cache,
    ILogger<GetApartmentsHandler>  logger)
{
    public async Task<Result<PageResult<ApartmentListItem>>> HandleAsync(GetApartmentsQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await cache.GetOrSetAsync(
                    ApartmentCachePolicy.PagedKey(query.Page, query.PageSize),
                    async ct =>
                    {
                        await using var context = await dbContextFactory.CreateDbContextAsync(ct);

                        var totalCount = await context.Apartments.CountAsync(ct);

                        var items = await context.Apartments
                            .AsNoTracking()
                            .OrderBy(a => a.Name)
                            .Skip((query.Page - 1) * query.PageSize)
                            .Take(query.PageSize)
                            .Select(a => new ApartmentListItem(
                                a.Id,
                                a.Name,
                                a.Details,
                                a.GroupId,
                                a.Group!.Name,
                                a.PublicToken,
                                !string.IsNullOrWhiteSpace(a.PublicToken),
                                a.Reads.Any()))
                            .ToListAsync(ct);

                        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

                        return new PageResult<ApartmentListItem>(
                            items,
                            query.Page,
                            query.PageSize,
                            totalCount,
                            totalPages);
                    },
                    TimeSpan.FromMinutes(30),
                    [ApartmentCachePolicy.Tag],
                    cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occured when fetching list of apartments - {ErrorMessage} - {Error}", 
                ex.Message, 
                ex);
            return Errors.ApplicationError;
        }
    }
}