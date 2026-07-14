using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Domain.Reads;
using WaterCalculator.Features.Views;

namespace WaterCalculator.Features.Payoffs.Get
{
    public sealed class GetPayoffDetailsHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<GetPayoffDetailsHandler> logger)
    {
        public async Task<Result<PayoffDetails>> HandleAsync(Guid groupId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var payoff = await context.Payoffs
                    .AsNoTracking()
                    .Include(p => p.Reads)
                    .Include(p => p.Invoice)
                    .FirstOrDefaultAsync(
                        p => p.GroupId == groupId && p.Status != PayoffStatus.Settled, cancellationToken);

                if (payoff is null)
                    return Errors.NotFoundError;

                var groupApartments = await context.Apartments
                    .AsNoTracking()
                    .Where(a => a.GroupId == groupId)
                    .ToListAsync(cancellationToken);
                 
                if (groupApartments.Count == 0)
                    return Errors.InvalidOperationError("Błąd podczas mapowania mieszkań do odczytów.");

                var readsByApartmentId = payoff.Reads.ToDictionary(r => r.ApartmentId, r => (Read?)r);

                var apartments = groupApartments
                    .Select(apartment => new PayoffApartmentItem(
                        apartment.Id,
                        apartment.Name,
                        readsByApartmentId.TryGetValue(apartment.Id, out var read) ? read : null))
                    .ToList();

                return new PayoffDetails(
                    payoff,
                    apartments);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured during processing payoff's summary for group {GroupId} - {ErrorMessage} - {Exception}",
                    groupId, ex.Message, ex);
                return Errors.ApplicationError;
            }
        }
    }
}
