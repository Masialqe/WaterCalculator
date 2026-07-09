using System.ComponentModel.DataAnnotations;
using WaterCalculator.Components.Shared.Validators;

namespace WaterCalculator.Components.Invoices
{
    public sealed class InvoiceFormModel
    {
        [Required(ErrorMessage = "Nazwa faktry jest wymagana.")]
        [MaxLength(100, ErrorMessage = "Nazwa faktury może mieć maksymalnie 100 znaków.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Nazwa może zawierać tylko znaki i litery.")]
        [MinLength(2, ErrorMessage = "Nazwa musi zawierać conajmniej dwa znaki.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Number faktru jest wymagany.")]
        [MaxLength(100, ErrorMessage = "Numer faktury może mieć maksymalnie 50 znaków.")]
        [RegularExpression(@"^[a-zA-Z0-9 -/.]+$", ErrorMessage = "Numer może zawierać tylko znaki i litery.")]
        [MinLength(2, ErrorMessage = "Numer musi zawierać conajmniej dwa znaki.")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Suma faktury jest wymagana.")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena nie może być mniejsza niż zero.")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Zużycie faktury jest wymagana.")]
        [Range(0, int.MaxValue, ErrorMessage = "Zużycie nie może być mniejsza niż zero.")]
        public decimal TotalConsumption { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        public decimal PricePerUnit => TotalConsumption > 0 ? TotalPrice / TotalConsumption : 0;
    }
}