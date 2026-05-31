namespace WaterCalculator.Domain
{
    public sealed class Apartment : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
     
        public Guid? GroupId { get; set; }
        public Group? Group { get; set; }

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
