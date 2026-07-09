using System.ComponentModel.DataAnnotations.Schema;
using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Domain.Invoices
{
    public sealed class Invoice : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public decimal TotalConsumption { get; set; }
        public ICollection<Settlement> Settlements { get; set; } = [];
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = [];

        public Guid PayoffId { get; set; }
        public Payoff Payoff { get; set; } = null!;

        public DateTime InvoiceDate { get; set; }

        [NotMapped]
        public decimal InvoiceItemsSum => InvoiceItems.Sum(x => x.TotalBruttoPrice);

        public Invoice() {}
        public Invoice(
            string name, 
            string number, 
            decimal totalPrice, 
            decimal totalConsumption, 
            DateTime invoiceDate,
            Guid payoffId)
        {
            Name = name;
            Number = number;
            TotalPrice = totalPrice;
            TotalConsumption = totalConsumption;
            InvoiceDate = invoiceDate;
            PayoffId = payoffId;
        }

        public static Invoice Create(string name, string number, decimal totalPrice, 
            decimal totalConsumption, DateTime invoiceDate, Guid payoffId)
            => new(name, number, totalPrice, totalConsumption, invoiceDate, payoffId);

        public Result Validate()
        {
            if (!InvoiceItems.Any())
                return Errors.InvalidOperationError("Faktura musi zawierać przynajmniej jedną pozycję.");

            if (Math.Abs(InvoiceItemsSum - TotalPrice) > 0.01m)
                return Errors.InvalidOperationError("Niepoprawna suma faktury.");

            if(TotalPrice == 0 || TotalConsumption == 0)
                return Errors.InvalidOperationError("Faktura nie może być pusta.");

            return Result.Success();
        }

        public void AddInvoiceItem(InvoiceItem item)
        {
            item.InvoiceId = Id;
            InvoiceItems.Add(item);
        }
    }
}

/*
 * Rozlicz grupe:
 * 1) Jest nierozliczona faktura 
 * 2) Wszystkie osoby, które są w grupie mają 1 nierozliczone rozliczenie 
 * 3) Liczymy zużycia czyli ostatnie - przedostatnie -> walidujemy ze zużyciem głównym
 * 4) Jeśli wynik wychodzi poza tolerancją -> decyzja usera czy kontynuować ??
 * 5) Dzielimy koszty do równego podziału / liczba osób w grupie
 * 6) Liczymy koszty z pozycji wedle zużycie (cena jednostkowa x zużycie dla każdego)
 * 7) Dodajemy koszty i tworzymy settlement dla każdego człowieka z grupy - faktura, zużycie, opłata
 * 8) Sumujemy koszty i weryfikujemy z fakturą - jest git to save 
 * 9) Reads -> Status na Calculated
 * 10) Invoice Status na Calculated
 * 11) Finito
 */