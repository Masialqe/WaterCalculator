using WaterCalculator.Domain;

namespace WaterCalculator.Features.Payoffs.GetSummary
{
    public sealed record PayoffSummaryItem(
        Guid ApartmentId,
        string ApartmentName,
        decimal Consumption,
        decimal AmountToPay,
        RealizationStatus Status);
}
