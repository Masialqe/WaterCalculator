using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Abstractions;
using WaterCalculator.Database;
using WaterCalculator.Domain;

namespace WaterCalculator.Features.Apartments.GetAsList;

public record GetAsListQuery(int Page, int PageSize);
public sealed class GetAsListHandler(IDbContextFactory<DatabaseContext> dbContextFactory, 
    ILogger<GetAsListHandler>  logger)
{
    public async Task<Result<List<Apartment>>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Apartments.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occured when fetching list of apartments - {ErrorMessage} - {Error}", ex.Message, ex);
            return Errors.ApplicationError;
        }
    }
}