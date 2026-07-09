using WaterCalculator.Components.InvoiceItems;
using WaterCalculator.Components.Invoices;
using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Features.Invoices.Create
{
    public sealed record CreateInvoiceCommand(InvoiceFormModel InvoiceData, 
        List<CreateInvoiceItemFormModel> InvoiceItems, Guid PayoffId)
    {
        public Invoice ToInvoice()
               => Invoice.Create(InvoiceData.Name, InvoiceData.Number, InvoiceData.TotalPrice, 
                   InvoiceData.TotalConsumption, InvoiceData.InvoiceDate, PayoffId);

        public List<InvoiceItem> ToInvoiceItems(Guid invoiceId)
                => InvoiceItems.Select(ii => InvoiceItem.Create(ii.Name, ii.Amount,
                    ii.PricePerUnit, (int)ii.Vat, ii.CalculationType, invoiceId)).ToList();
    }
}
