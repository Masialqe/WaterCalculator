namespace WaterCalculator.Domain
{
    public class Read : BaseEntity
    {
        public double Amount { get; set; }
        public Guid ApartmentId {  get; set; }
        public Apartment Apartment { get; set; } = null!;
        public decimal Value { get; set; }

    }
}
