using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Domain.Invoices;
using WaterCalculator.Domain.Reads;

namespace WaterCalculator.Features.Settlements
{
    public sealed record GenerateSettlementForGroupCommand(Guid GroupId);

    public class GenerateSettlementForGroupHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<GenerateSettlementForGroupHandler> logger)
    {
        public async Task<Result> HandleAsync(GenerateSettlementForGroupCommand command, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                //await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                //var groupUserCount = await context.Apartments
                //    .CountAsync(a => a.GroupId == command.GroupId, cancellationToken);

                //if (groupUserCount == 0) return Errors.InvalidOperationError("Grupa nie posiada mieszkań.");

                //if (!await context.Groups.AnyAsync(g => g.Id == command.GroupId) || 
                //    command.GroupId == Guid.Empty)
                //        return Errors.NotFoundError;

                //var groupInvoice = await context.Invoices.FirstOrDefaultAsync(i => 
                //    i.GroupId == command.GroupId && i.Status == InvoiceStatus.Pending);

                //if (groupInvoice is null) 
                //    return Errors.InvalidOperationError("Brak oczekujących faktur dla tej grupy.");

                //var apartmentsPendingReads = await context.Apartments
                //    .Where(x => x.GroupId == command.GroupId)
                //    .CountAsync(x => x.Reads.Any(r => r.Status == ReadStatus.Pending));

                //if (groupUserCount != apartmentsPendingReads) 
                //    return Errors.InvalidOperationError("Nie wszystkie mieszkania mają oczekujące odczyty.");

                //var reads = await context.Reads
                //    .Where(r => r.Status == ReadStatus.Pending &&
                //     r.Apartment.GroupId == command.GroupId)
                //    .ToListAsync(cancellationToken);



                return Result.Success();

            }
            catch (Exception ex)
            {
                logger.LogError("An error occured calculating settlements for group {GroupId} - {ErrorMessage} - {Error}.",
                    command.GroupId,
                    ex.Message,
                    ex);

                return Errors.ApplicationError;
            }
        }
    }
}
