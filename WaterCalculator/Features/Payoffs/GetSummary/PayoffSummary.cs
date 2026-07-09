namespace WaterCalculator.Features.Payoffs.GetSummary
{
    public sealed record PayoffSummary(
        Guid PayoffId,
        string GroupName, 
        DateTime DateFrom,
        DateTime DateTo,
        decimal TotalConsumption,
        decimal TotalAmountToPay,
        int SettlementsCount,
        List<PayoffSummaryItem> SummaryItems);
}
