using WaterCalculator.Domain.Invoices;
using WaterCalculator.Domain.Reads;
using WaterCalculator.Features.Payoffs.Settle;


namespace WaterCalculator.Tests.Features.Payoff
{
    public sealed class PayoffSettlementCalculatorTests
    {
        private readonly PayoffSettlementCalculator _calculator = new();

        [Fact]
        public void CalculateSettlements_ShouldFail_WhenPayoffHasNoReads()
        {
            var payoff = new WaterCalculator.Domain.Payoff
            {
                Invoice = CreateInvoice(100m, CreateEqualSplitItem(100m)),
                Reads = []
            };

            var result = _calculator.CalculateSettlements(payoff);

            Assert.True(result.IsFailure);
            Assert.Equal("Brak odczytów do rozliczenia.", result.Error.ErrorDescription);
        }

        [Fact]
        public void CalculateSettlements_ShouldFail_WhenInvoiceHasNoItems()
        {
            var payoff = new WaterCalculator.Domain.Payoff
            {
                Invoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    TotalPrice = 100m,
                    InvoiceItems = []
                },
                Reads = [CreateRead(Guid.NewGuid(), 10m)]
            };

            var result = _calculator.CalculateSettlements(payoff);

            Assert.True(result.IsFailure);
            Assert.Equal("Brak wpisów na fakturze.", result.Error.ErrorDescription);
        }

        [Fact]
        public void CalculateSettlements_ShouldFail_WhenPerConsumptionExistsAndTotalConsumptionIsZero()
        {
            var payoff = new WaterCalculator.Domain.Payoff
            {
                Invoice = CreateInvoice(100m, CreatePerConsumptionItem(totalBruttoPrice: 100m, bruttoPricePerUnit: 4.60m)),
                Reads =
                [
                    CreateRead(Guid.NewGuid(), 0m),
                CreateRead(Guid.NewGuid(), 0m)
                ]
            };

            var result = _calculator.CalculateSettlements(payoff);

            Assert.True(result.IsFailure);
            Assert.Equal("Nie można rozliczyć pozycji zależnych od zużycia bez zużycia.", result.Error.ErrorDescription);
        }

        [Fact]
        public void CalculateSettlements_ShouldCalculateOnlyEqualSplitItems()
        {
            var apartment1 = Guid.NewGuid();
            var apartment2 = Guid.NewGuid();
            var apartment3 = Guid.NewGuid();
            var apartment4 = Guid.NewGuid();

            var payoff = new WaterCalculator.Domain.Payoff
            {
                Invoice = CreateInvoice(40m, CreateEqualSplitItem(40m)),
                Reads =
                [
                    CreateRead(apartment1, 10m),
                CreateRead(apartment2, 20m),
                CreateRead(apartment3, 30m),
                CreateRead(apartment4, 40m)
                ]
            };

            var result = _calculator.CalculateSettlements(payoff);

            Assert.True(result.IsSuccess);

            var settlements = result.Value;
            Assert.Equal(4, settlements.Count);
            Assert.All(settlements, settlement => Assert.Equal(10m, settlement.AmountToPay));
            Assert.Equal(40m, settlements.Sum(x => x.AmountToPay));
        }

        [Fact]
        public void CalculateSettlements_ShouldCalculateOnlyPerConsumptionItems()
        {
            var apartment1 = Guid.NewGuid();
            var apartment2 = Guid.NewGuid();
            var apartment3 = Guid.NewGuid();
            var apartment4 = Guid.NewGuid();

            var payoff = new WaterCalculator.Domain.Payoff
            {
                Invoice = CreateInvoice(
                    460m,
                    CreatePerConsumptionItem(totalBruttoPrice: 460m, bruttoPricePerUnit: 4.60m)),
                Reads =
                [
                    CreateRead(apartment1, 10m),
                CreateRead(apartment2, 20m),
                CreateRead(apartment3, 30m),
                CreateRead(apartment4, 40m)
                ]
            };

            var result = _calculator.CalculateSettlements(payoff);

            Assert.True(result.IsSuccess);

            var settlements = result.Value;
            Assert.Equal(46m, settlements.Single(x => x.ApartmentId == apartment1).AmountToPay);
            Assert.Equal(92m, settlements.Single(x => x.ApartmentId == apartment2).AmountToPay);
            Assert.Equal(138m, settlements.Single(x => x.ApartmentId == apartment3).AmountToPay);
            Assert.Equal(184m, settlements.Single(x => x.ApartmentId == apartment4).AmountToPay);
            Assert.Equal(460m, settlements.Sum(x => x.AmountToPay));
        }

        [Fact]
        public void CalculateSettlements_ShouldCalculateEqualSplitAndPerConsumptionTogether()
        {
            var apartment1 = Guid.NewGuid();
            var apartment2 = Guid.NewGuid();
            var apartment3 = Guid.NewGuid();
            var apartment4 = Guid.NewGuid();

            var payoff = new WaterCalculator.Domain.Payoff
            {
                Invoice = CreateInvoice(
                    500m,
                    CreateEqualSplitItem(40m),
                    CreatePerConsumptionItem(totalBruttoPrice: 460m, bruttoPricePerUnit: 4.60m)),
                Reads =
                [
                    CreateRead(apartment1, 10m),
                CreateRead(apartment2, 20m),
                CreateRead(apartment3, 30m),
                CreateRead(apartment4, 40m)
                ]
            };

            var result = _calculator.CalculateSettlements(payoff);

            Assert.True(result.IsSuccess);

            var settlements = result.Value;
            Assert.Equal(56m, settlements.Single(x => x.ApartmentId == apartment1).AmountToPay);
            Assert.Equal(102m, settlements.Single(x => x.ApartmentId == apartment2).AmountToPay);
            Assert.Equal(148m, settlements.Single(x => x.ApartmentId == apartment3).AmountToPay);
            Assert.Equal(194m, settlements.Single(x => x.ApartmentId == apartment4).AmountToPay);
            Assert.Equal(500m, settlements.Sum(x => x.AmountToPay));
        }

        [Fact]
        public void CalculateSettlements_ShouldPreserveConsumptionPerApartment()
        {
            var apartmentId = Guid.NewGuid();

            var payoff = new WaterCalculator.Domain.Payoff
            {
                Invoice = CreateInvoice(
                    46m,
                    CreatePerConsumptionItem(totalBruttoPrice: 46m, bruttoPricePerUnit: 4.60m)),
                Reads =
                [
                    CreateRead(apartmentId, 10m)
                ]
            };

            var result = _calculator.CalculateSettlements(payoff);

            Assert.True(result.IsSuccess);

            var settlement = Assert.Single(result.Value);
            Assert.Equal(apartmentId, settlement.ApartmentId);
            Assert.Equal(10m, settlement.Consumption);
            Assert.Equal(46m, settlement.AmountToPay);
        }

        private static Invoice CreateInvoice(decimal totalPrice, params InvoiceItem[] items)
        {
            return new Invoice
            {
                Id = Guid.NewGuid(),
                TotalPrice = totalPrice,
                InvoiceItems = items.ToList()
            };
        }

        private static InvoiceItem CreateEqualSplitItem(decimal totalBruttoPrice)
        {
            return new InvoiceItem
            {
                Name = "Koszt staly",
                CalculationType = CalculationType.EqualSplit,
                TotalBruttoPrice = totalBruttoPrice,
                BruttoPricePerUnit = 0m
            };
        }

        private static InvoiceItem CreatePerConsumptionItem(decimal totalBruttoPrice, decimal bruttoPricePerUnit)
        {
            return new InvoiceItem
            {
                Name = "Woda",
                CalculationType = CalculationType.PerConsumption,
                TotalBruttoPrice = totalBruttoPrice,
                BruttoPricePerUnit = bruttoPricePerUnit
            };
        }

        private static Read CreateRead(Guid apartmentId, decimal consumptionFromLastRead)
        {
            return new Read
            {
                ApartmentId = apartmentId,
                ConsumptionFromLastRead = consumptionFromLastRead
            };
        }
    }
}
