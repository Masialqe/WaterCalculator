using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using WaterCalculator.Domain;

namespace WaterCalculator.Features.Apartments.GenerateAccess
{
    public static class AccessCodeProvider
    {
        private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private const int DefaultLength = 8;

        
        public static string Generate(int length = DefaultLength)
        {
            if (length < 6)
                throw new ArgumentOutOfRangeException(nameof(length), "Kod powinien zawierać minimum 6 znaków.");

            var result = new StringBuilder();

            for(var i = 0; i < length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(Alphabet.Length);
                result.Append(Alphabet[index]);
            }

            return result.ToString();
        }
    }
}
