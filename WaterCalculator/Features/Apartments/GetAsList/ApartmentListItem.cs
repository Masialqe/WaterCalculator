namespace WaterCalculator.Features.Apartments.GetAsList
{
    public sealed record ApartmentListItem(
        Guid Id,
        string Name,
        string Details,
        Guid? GroupId,
        string GroupName,
        string? PublicToken,
        bool HasAccessConfigured,
        bool HasAnyRead);
}
