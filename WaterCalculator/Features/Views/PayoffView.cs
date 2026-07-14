using WaterCalculator.Domain;

namespace WaterCalculator.Features.Views;

public sealed record PayoffView(
    Guid Id,
    DateTime PeriodFrom,
    DateTime PeriodTo,
    PayoffStatus Status,
    decimal TotalMeterValue,
    decimal TotalConsumptionValue,
    InvoiceView? Invoice
)
{
    
    public static implicit operator PayoffView(Payoff payoff)
        => new(
            payoff.Id,
            payoff.PeriodFrom,
            payoff.PeriodTo,
            payoff.Status,
            payoff.TotalMeterValue,
            payoff.TotalConsumptionValue,
            payoff.Invoice!);
}