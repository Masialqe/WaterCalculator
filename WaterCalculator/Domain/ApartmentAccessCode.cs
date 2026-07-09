using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WaterCalculator.Domain
{
    public class ApartmentAccessCode : BaseEntity
    {
        public Guid ApartmenId { get; set; }
        public Apartment? Apartment { get; set; }
        public string Code { get; set; } = string.Empty;

        public ApartmentAccessCode() { }
        public ApartmentAccessCode(string code, Guid apartmentId)
        {
            Code = code;
            ApartmenId = apartmentId;
        }

        public static ApartmentAccessCode Create(string code, Guid apartmentId)
         => new ApartmentAccessCode(code, apartmentId);
    }
}
