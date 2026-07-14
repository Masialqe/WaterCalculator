using WaterCalculator.Features.Views;

namespace WaterCalculator.Features.Payoffs.GetSummary
{
    public sealed record PayoffSummary(
        string GroupName, 
        decimal TotalAmountToPay,
        int SettlementsCount,
        PayoffView Payoff,
        List<PayoffSummaryItem> SummaryItems);
}
