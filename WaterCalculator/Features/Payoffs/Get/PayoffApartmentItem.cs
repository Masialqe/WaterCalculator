using WaterCalculator.Domain.Reads;

namespace WaterCalculator.Features.Payoffs.Get
{
    public sealed record PayoffApartmentItem(Guid ApartmentId, string ApartmentName, Read? Read);
    
}
