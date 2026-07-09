using System.ComponentModel.DataAnnotations.Schema;
using WaterCalculator.Domain.Reads;

namespace WaterCalculator.Domain
{
    public sealed class Apartment : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
     
        public Guid? GroupId { get; set; }
        public Group? Group { get; set; }

        public string? PublicToken { get; set; }

        public Guid? AccessCodeId { get; set; }
        public ApartmentAccessCode? AccessCode { get; set; }

        [NotMapped]
        public bool HasAccessConfigured =>
            !string.IsNullOrWhiteSpace(PublicToken);

        [NotMapped]
        public string AccessCodeValue => AccessCode?.Code ?? string.Empty;

        public ICollection<Read> Reads { get; set; } = [];
        public ICollection<Settlement> Settlements { get; set; } = [];

        public Apartment(){}
        public Apartment(string name, string details, Guid? groupId)
        {
            Name = name;
            Details = details;
            GroupId = groupId;
        }
        
        public static Apartment Create(string name, string details, Guid? groupId)
         => new Apartment(name, details, groupId);
    }
}
