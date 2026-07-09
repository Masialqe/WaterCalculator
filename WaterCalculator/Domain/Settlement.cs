using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Domain
{
    public class Settlement : BaseEntity
    {
        public Guid ApartmentId { get; set; }
        public Apartment Apartment { get; set; } = null!;

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;

        public Guid PayoffId { get; set; }
        public Payoff Payoff { get; set; } = null!;

        public decimal Consumption { get; set; }
        public decimal AmountToPay { get; set; }

        public RealizationStatus RealizationStatus { get; set; } = RealizationStatus.Pending;

        public Settlement() { }

        public Settlement(
            Guid apartmentId, 
            Guid invoiceId, 
            Guid payoffId,
            decimal consumption, 
            decimal amountToPay)
        {
            ApartmentId = apartmentId;
            InvoiceId = invoiceId;
            PayoffId = payoffId;
            Consumption = consumption;
            AmountToPay = amountToPay;
        }

        public static Settlement Create(
            Guid apartmentId, 
            Guid invoiceId,
            Guid payoffId,
            decimal consumption, 
            decimal amountToPay)
            => new(apartmentId, invoiceId, payoffId, consumption, amountToPay);
    }
}
