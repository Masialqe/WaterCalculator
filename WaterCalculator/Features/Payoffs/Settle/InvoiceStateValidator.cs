using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Features.Payoffs.Settle
{
    public sealed class InvoiceStateValidator
    {
        public Result Validate(Payoff payoff)
        {
            if (payoff.Invoice is null)
                return Errors.InvalidOperationError("Brak przypisanej faktury.");

            var invoice = payoff.Invoice;

            if (Math.Abs(payoff.TotalMeterValue - invoice.TotalConsumption) > 1m)
                return Errors.InvalidOperationError("Niezgodny stan zużycia.");

            return Result.Success();
        }
    }
}
