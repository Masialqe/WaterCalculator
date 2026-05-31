namespace WaterCalculator.Domain
{
    public class Read : BaseEntity
    {
        public Guid ApartmentId {  get; set; }
        public Apartment Apartment { get; set; } = null!;
        public decimal Value { get; set; }
        
        public DateTime ReadDate { get; set; }

        public Read(){}

        public Read(decimal value, Guid apartmentId)
        {
            Value = value;
            ApartmentId = apartmentId;
        }
        
        public static Read Create(decimal value, Guid apartmentId)
            => new Read(value,apartmentId);
    }
}
