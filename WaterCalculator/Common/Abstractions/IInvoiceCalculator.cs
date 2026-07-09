using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Common.Abstractions
{
    public interface IInvoiceCalculator<TResult>
    {
        Task<TResult> CalculateAsync(Invoice invoice, 
            CancellationToken cancellationToken = default);
    }
}
