namespace WaterCalculator.Features.Payoffs.GetList;

public sealed record PayoffListItem(
    Guid PayoffId,
    string GroupName, 
    DateTime PeriodFrom,
    DateTime PeriodTo,
    decimal TotalMeterValue,
    decimal TotaConsumption);