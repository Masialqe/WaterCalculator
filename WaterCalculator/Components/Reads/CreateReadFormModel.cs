using System.ComponentModel.DataAnnotations;

namespace WaterCalculator.Components.Reads
{
    internal sealed class CreateReadFormModel
    {
        [Required(ErrorMessage = "Wartość jest wymagana.")]
        [Range(0, int.MaxValue, ErrorMessage = "Wartość nie może być niższa niż 0.")]
        public decimal Value { get; set; } = 0;
        public DateTime ReadDate { get; set; } = DateTime.UtcNow;
        public Guid ApartmentId { get; set; }
    }
}
