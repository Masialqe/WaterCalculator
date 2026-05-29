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

    }
}
