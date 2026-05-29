namespace WaterCalculator.Domain
{
    public class Settlement : BaseEntity
    {
        public Guid ApartmentId { get; set; }
        public Apartment Apartment { get; set; } = null!;

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;

        public decimal Consumption { get; set; }
        public decimal AmountToPay { get; set; }

        public RealizationStatus RealizationStatus { get; set; } = RealizationStatus.Pending;
    }
}
