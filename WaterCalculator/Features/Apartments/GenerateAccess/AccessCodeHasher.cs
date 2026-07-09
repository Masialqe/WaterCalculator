using Microsoft.AspNetCore.Identity;
using WaterCalculator.Domain;

namespace WaterCalculator.Features.Apartments.GenerateAccess
{
    public sealed class AccessCodeHasher
    {
        private readonly PasswordHasher<Apartment> _passwordHasher = new();

        public string Hash(Apartment apartment, string code)
                => _passwordHasher.HashPassword(apartment, code);

        public bool Verify(Apartment apartment, string code)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                apartment,
                apartment.AccessCodeValue,
                code);

            return result == PasswordVerificationResult.Success;
        }
    }
}
