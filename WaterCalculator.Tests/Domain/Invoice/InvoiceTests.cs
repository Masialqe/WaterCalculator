namespace WaterCalculator.Tests.Domain.Invoice
{
    public sealed class InvoiceTests
    {
        [Fact]
        public void Create_ShouldPopulateProperties()
        {
            var payoffId = Guid.NewGuid();
            var invoiceDate = new DateTime(2026, 6, 28);

            var invoice = WaterCalculator.Domain.Invoices.Invoice.Create(
                "Faktura za wode",
                "FV/06/2026/001",
                123.45m,
                67.89m,
                invoiceDate,
                payoffId);

            Assert.Equal("Faktura za wode", invoice.Name);
            Assert.Equal("FV/06/2026/001", invoice.Number);
            Assert.Equal(123.45m, invoice.TotalPrice);
            Assert.Equal(67.89m, invoice.TotalConsumption);
            Assert.Equal(invoiceDate, invoice.InvoiceDate);
            Assert.Equal(payoffId, invoice.PayoffId);
        }

        [Fact]
        public void Validate_ShouldFail_WhenInvoiceDoesNotContainAnyItems()
        {
            var invoice = CreateInvoice(totalPrice: 100m, totalConsumption: 50m);

            var result = invoice.Validate();

            Assert.True(result.IsFailure);
            Assert.Equal("Faktura musi zawierać przynajmniej jedną pozycję.", result.Error.ErrorDescription);
        }

        [Fact]
        public void Validate_ShouldFail_WhenInvoiceItemsSumDoesNotMatchTotalPrice()
        {
            var invoice = CreateInvoice(totalPrice: 100m, totalConsumption: 50m);
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: 49.99m));

            var result = invoice.Validate();

            Assert.True(result.IsFailure);
            Assert.Equal("Niepoprawna suma faktury.", result.Error.ErrorDescription);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 0)]
        [InlineData(0, 0)]
        public void Validate_ShouldFail_WhenInvoiceIsEmpty(decimal totalPrice, decimal totalConsumption)
        {
            var invoice = CreateInvoice(totalPrice, totalConsumption);
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: totalPrice));

            var result = invoice.Validate();

            Assert.True(result.IsFailure);
            Assert.Equal("Faktura nie może być pusta.", result.Error.ErrorDescription);
        }

        [Fact]
        public void Validate_ShouldSucceed_WhenInvoiceContainsItemsAndSumMatchesTotalPrice()
        {
            var invoice = CreateInvoice(totalPrice: 123.45m, totalConsumption: 10m);
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: 100m));
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: 23.45m));

            var result = invoice.Validate();

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Validate_ShouldSucceed_WhenDifferenceBetweenItemsSumAndTotalPriceIsWithinTolerance()
        {
            var invoice = CreateInvoice(totalPrice: 100m, totalConsumption: 5m);
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: 100.009m));

            var result = invoice.Validate();

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void AddInvoiceItem_ShouldAssignInvoiceIdAndAddItemToCollection()
        {
            var invoice = CreateInvoice(totalPrice: 50m, totalConsumption: 5m);
            invoice.Id = Guid.NewGuid();
            var item = CreateInvoiceItem(totalBruttoPrice: 50m, invoiceId: Guid.Empty);

            invoice.AddInvoiceItem(item);

            Assert.Single(invoice.InvoiceItems);
            Assert.Equal(invoice.Id, item.InvoiceId);
        }

        [Fact]
        public void InvoiceItemsSum_ShouldReturnSumOfAllItemsBruttoValues()
        {
            var invoice = CreateInvoice(totalPrice: 0m, totalConsumption: 1m);
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: 10m));
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: 15.55m));
            invoice.AddInvoiceItem(CreateInvoiceItem(totalBruttoPrice: 4.45m));

            Assert.Equal(30.00m, invoice.InvoiceItemsSum);
        }

        private static WaterCalculator.Domain.Invoices.Invoice CreateInvoice(decimal totalPrice, decimal totalConsumption)
            => WaterCalculator.Domain.Invoices.Invoice.Create(
                "Test invoice",
                "FV/TEST/001",
                totalPrice,
                totalConsumption,
                new DateTime(2026, 6, 28),
                Guid.NewGuid());

        private static WaterCalculator.Domain.Invoices.InvoiceItem CreateInvoiceItem(decimal totalBruttoPrice, Guid? invoiceId = null)
        {
            var item = WaterCalculator.Domain.Invoices.InvoiceItem.Create(
                "Zimna woda",
                1m,
                1m,
                23,
                WaterCalculator.Domain.Invoices.CalculationType.PerConsumption,
                invoiceId ?? Guid.NewGuid());

            item.TotalBruttoPrice = totalBruttoPrice;
            return item;
        }
    }

}
