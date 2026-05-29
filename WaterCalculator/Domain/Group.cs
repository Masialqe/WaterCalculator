namespace WaterCalculator.Domain
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public ICollection<Apartment> Apartments { get; set; } = [];
    }
}
