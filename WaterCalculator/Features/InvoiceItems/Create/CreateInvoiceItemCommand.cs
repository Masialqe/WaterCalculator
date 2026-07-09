using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Features.InvoiceItems.Create
{
    public sealed record CreateInvoiceItemCommand(string Name, decimal Amount, decimal PricePerUnit, decimal Vat, 
        CalculationType CalculationType, Guid InvoiceId);
}
