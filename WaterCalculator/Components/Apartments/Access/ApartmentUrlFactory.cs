using Microsoft.AspNetCore.Components;
using WaterCalculator.Domain;
using WaterCalculator.Features.Apartments.GetAsList;

namespace WaterCalculator.Components.Apartments.Access
{
    public class ApartmentUrlFactory(
        NavigationManager navigation)

    {
        public string GetApartmentAccessUrl(string publicToken)
            => string.IsNullOrWhiteSpace(publicToken)
                ? string.Empty
                : $"{navigation.BaseUri.TrimEnd('/')}/apartments/{publicToken}/access";

        public string GetUrlTokenText(
            Apartment apartment,
            int maxLength = 20)
            => apartment.HasAccessConfigured
                ? $"/{apartment.PublicToken}".Substring(0, maxLength)
                : "Brak url";

        public string GetUrlTokenText(
            ApartmentListItem apartment,
            int maxLength = 20)
            => apartment.HasAccessConfigured
                ? $"/{apartment.PublicToken}".Substring(0, maxLength)
                : "Brak url";


    }
}

