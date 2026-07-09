using System.ComponentModel.DataAnnotations.Schema;

namespace WaterCalculator.Domain.Invoices
{
    public class InvoiceItem : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; } 
        public decimal PricePerUnit { get; set; }
        public decimal BruttoPricePerUnit { get; set; }
        public decimal TotalNettoPrice { get; set; }
        public decimal TotalBruttoPrice { get; set; }
        public int Vat { get; set; }
        public CalculationType CalculationType { get; set; }

        public InvoiceItem() {}
        public InvoiceItem(string name, decimal amount, decimal pricePerUnit, 
            int vat, CalculationType calculationType, Guid invoiceId)
        {
            Name = name;
            Amount = amount;
            PricePerUnit = pricePerUnit;
            Vat = vat;
            CalculationType = calculationType;
            InvoiceId = invoiceId;
            TotalNettoPrice = Amount * PricePerUnit;
            TotalBruttoPrice = GetBruttoPrice(TotalNettoPrice);
            BruttoPricePerUnit = GetBruttoPrice(PricePerUnit);
        }

        public static InvoiceItem Create(string name, decimal amount, decimal pricePerUnit,
            int vat, CalculationType calculationType, Guid invoiceId)
                => new(name, amount, pricePerUnit, vat, calculationType, invoiceId);

        private decimal GetBruttoPrice(decimal value)
            => value * (1 + Vat / 100m);
    }
}
