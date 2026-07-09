namespace WaterCalculator.Features.Apartments.GenerateAccess
{
    public sealed record RegenerateAccessCodeCommand(Guid ApartmentId, string PublicToken);
}
