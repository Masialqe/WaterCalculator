namespace WaterCalculator.Features.Apartments.GetAsList
{
    public sealed record ApartmentListItem(
        Guid Id,
        string Name,
        string Details,
        Guid? GroupId,
        string? PublicToken,
        bool HasAccessConfigured,
        bool HasAnyRead);
}
