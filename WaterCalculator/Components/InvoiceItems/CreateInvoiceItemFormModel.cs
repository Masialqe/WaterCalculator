using System.ComponentModel.DataAnnotations;
using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Components.InvoiceItems
{
    public sealed class CreateInvoiceItemFormModel
    {
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [MaxLength(100, ErrorMessage = "Nazwa może mieć maksymalnie 100 znaków.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Nazwa może zawierać tylko znaki i litery.")]
        [MinLength(2, ErrorMessage = "Nazwa musi zawierać conajmniej dwa znaki.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = " Ilość jest wymagana.")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena nie może być mniejsza niż zero.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = " Cena jednostkowa jest wymagana.")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena nie może być mniejsza niż zero.")]
        public decimal PricePerUnit { get; set; }
        public decimal TotalNettoPrice => Amount * PricePerUnit;
        public decimal TotalBruttoPrice => Vat > 0 ? TotalNettoPrice * (1 + Vat / 100) : TotalNettoPrice;

        [Required(ErrorMessage = " Stawka VAT jest wymagana.")]
        [Range(0, 23, ErrorMessage = "Stawka VAT musi być liczbą z zakresu 0-23.")]
        public decimal Vat { get; set; }
        public CalculationType CalculationType { get; set; }
    }
}
