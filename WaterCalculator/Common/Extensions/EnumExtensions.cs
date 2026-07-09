using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WaterCalculator.Common.Extensions
{
    public static class EnumExtensions
    {
        extension(Enum @enum)
        {
            public string GetDisplayName()
            {
                var member = @enum.GetType().GetMember(@enum.ToString()).FirstOrDefault();
                var displayAttr = member?.GetCustomAttribute<DisplayAttribute>();

                return displayAttr?.Name ?? @enum.ToString();
            }
        }
    }
}
