using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Features.Payoffs.Settle
{
    public class PayoffSettlementCalculator
    {
        private const decimal Tolerance = 0.01m;
        public Result<List<Settlement>> CalculateSettlements(Payoff payoff)
        {
            var validationResult = ValidateInput(payoff);
            if (validationResult.IsFailure)
                return validationResult.Error;

            var invoice = payoff.Invoice!;
            var reads = payoff.Reads.ToList();
            var invoiceItems = invoice.InvoiceItems.ToList();

            var perConsumptionItems = invoiceItems
                .Where(x => x.CalculationType == CalculationType.PerConsumption)
                .ToList();

            var equalSplitItems = invoiceItems
                .Where(x => x.CalculationType == CalculationType.EqualSplit)
                .ToList();

            var totalConsumption = reads.Sum(x => x.ConsumptionFromLastRead);
            var equalSplitAmountPerApartment = equalSplitItems.Sum(x => x.TotalBruttoPrice) / reads.Count;

            if (perConsumptionItems.Count > 0 && totalConsumption <= 0)
                return Errors.InvalidOperationError("Nie można rozliczyć pozycji zależnych od zużycia bez zużycia.");

            var settlements = new List<Settlement>();
           
            foreach (var read in reads)
            {
                var consumptionBasedAmount = perConsumptionItems.Sum(item =>
                    read.ConsumptionFromLastRead * item.BruttoPricePerUnit);

                var amountToPay = decimal.Round(
                    equalSplitAmountPerApartment + consumptionBasedAmount,
                    2,
                    MidpointRounding.AwayFromZero);

                settlements.Add(Settlement.Create(
                    read.ApartmentId,
                    invoice.Id,
                    payoff.Id,
                    read.ConsumptionFromLastRead,
                    amountToPay));
            }

            var totalSettlementsValue = settlements.Sum(s => s.AmountToPay);

            if(Math.Abs(totalSettlementsValue - invoice.TotalPrice) > Tolerance)
                return Errors.InvalidOperationError($"Wyliczona kwota ({totalSettlementsValue}) nie zgadza się z kwotą faktury ({invoice.TotalPrice}).");

            return settlements;
        }

        private Result ValidateInput(Payoff payoff)
        {
            if (payoff.Invoice is null)
                return Errors.InvalidOperationError("Brak przypisanej faktury.");

            if (payoff.Reads.Count == 0)
                return Errors.InvalidOperationError("Brak odczytów do rozliczenia.");

            if (payoff.Invoice.InvoiceItems.Count == 0)
                return Errors.InvalidOperationError("Brak wpisów na fakturze.");

            return Result.Success();
        }
    }
}
