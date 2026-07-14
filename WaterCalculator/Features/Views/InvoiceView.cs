using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Features.Views;

public sealed record InvoiceView(
    Guid Id,
    string InvoiceNumber,
    string InvoiceName,
    decimal MeterValue,
    decimal TotalAmount
)
{
    public static implicit operator InvoiceView?(Invoice? invoice)
        => invoice is null
            ? null
            : new InvoiceView(
                invoice.Id,
                invoice.Number,
                invoice.Name,
                invoice.TotalConsumption,
                invoice.TotalPrice);
}