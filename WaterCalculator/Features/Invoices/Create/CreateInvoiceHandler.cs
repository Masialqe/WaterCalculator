using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Features.Invoices.Create
{
    public sealed class CreateInvoiceHandler(IDbContextFactory<DatabaseContext> dbContextFactory, 
        ILogger<CreateInvoiceHandler> logger)
    {
        public async Task<Result> HandleAsync(CreateInvoiceCommand command,
            CancellationToken cancellationToken = default)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var invoiceData = command.InvoiceData;

                if (await context.Invoices.AnyAsync(i => i.PayoffId == command.PayoffId))
                    return Errors.InvalidOperationError("Dla tego rozliczenia została dodana faktura.");

                if (await context.Payoffs.AnyAsync(p => p.Status != PayoffStatus.Summarized
                    && p.Id == command.PayoffId))
                    return Errors.InvalidOperationError("Na tym etapie nie można dodać faktury.");

                var invoice = command.ToInvoice();

                foreach (var item in command.InvoiceItems)
                {
                    invoice.AddInvoiceItem(InvoiceItem.Create(
                        item.Name,
                        item.Amount,
                        item.PricePerUnit,
                        (int)item.Vat,
                        item.CalculationType,
                        invoice.Id));
                }

                var validation = invoice.Validate();
                if (validation.IsFailure) return validation.Error;

                context.Invoices.Add(invoice);

                await context.Payoffs.Where(p => p.Id == command.PayoffId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.Status, PayoffStatus.Unsettled), cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while saving invoice for {PayoffId} - Message: {Message}", 
                    command.PayoffId,
                    ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                return Errors.ApplicationError;
            }
        }
    }
}
