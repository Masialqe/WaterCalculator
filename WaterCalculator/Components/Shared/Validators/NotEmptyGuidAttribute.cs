using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WaterCalculator.Components.Shared.Validators
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value is Guid guid && guid !=Guid.Empty;
        }
    }
}
