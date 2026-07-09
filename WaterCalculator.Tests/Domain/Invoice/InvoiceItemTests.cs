using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Tests.Domain.Invoice
{
    public sealed class InvoiceItemTests
    {
        [Fact]
        public void Create_ShouldPopulateProperties()
        {
            var invoiceId = Guid.NewGuid();

            var item = InvoiceItem.Create(
                "Podgrzanie wody",
                10m,
                2.5m,
                23,
                CalculationType.PerConsumption,
                invoiceId);

            Assert.Equal("Podgrzanie wody", item.Name);
            Assert.Equal(10m, item.Amount);
            Assert.Equal(2.5m, item.PricePerUnit);
            Assert.Equal(23, item.Vat);
            Assert.Equal(CalculationType.PerConsumption, item.CalculationType);
            Assert.Equal(invoiceId, item.InvoiceId);
        }

        [Fact]
        public void Constructor_ShouldCalculateTotalNettoPrice()
        {
            var item = InvoiceItem.Create(
                "Zimna woda",
                12m,
                3.50m,
                23,
                CalculationType.PerConsumption,
                Guid.NewGuid());

            Assert.Equal(42.00m, item.TotalNettoPrice);
        }

        [Fact]
        public void Constructor_ShouldCalculateTotalBruttoPrice_ForPositiveVat()
        {
            var item = InvoiceItem.Create(
                "Scieki",
                10m,
                4.00m,
                23,
                CalculationType.PerConsumption,
                Guid.NewGuid());

            Assert.Equal(40.00m, item.TotalNettoPrice);
            Assert.Equal(49.20m, item.TotalBruttoPrice);
        }

        [Fact]
        public void Constructor_ShouldCalculateTotalBruttoPrice_ForZeroVat()
        {
            var item = InvoiceItem.Create(
                "Oplata stala",
                2m,
                15.00m,
                0,
                CalculationType.PerConsumption,
                Guid.NewGuid());

            Assert.Equal(30.00m, item.TotalNettoPrice);
            Assert.Equal(30.00m, item.TotalBruttoPrice);
        }

        [Fact]
        public void Constructor_ShouldCalculateTotalBruttoPrice_WithDecimalPrecision()
        {
            var item = InvoiceItem.Create(
                "Pozycja testowa",
                1.5m,
                9.99m,
                8,
                CalculationType.PerConsumption,
                Guid.NewGuid());

            Assert.Equal(14.985m, item.TotalNettoPrice);
            Assert.Equal(16.1838m, item.TotalBruttoPrice);
        }
    }
}
