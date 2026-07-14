using WaterCalculator.Domain;
using WaterCalculator.Features.Payoffs.GetSummary;
using WaterCalculator.Features.Views;

namespace WaterCalculator.Features.Payoffs.Get;

public sealed record PayoffDetails(
    PayoffView Payoff, 
    List<PayoffApartmentItem> Apartments);