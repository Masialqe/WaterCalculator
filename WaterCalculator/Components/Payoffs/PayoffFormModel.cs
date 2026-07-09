using System.ComponentModel.DataAnnotations;

namespace WaterCalculator.Components.Payoffs
{
    public class PayoffFormModel
    {
        [Required(ErrorMessage = "Data początkowa jest wymagana.")]
        public DateTime PayoffFrom { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Data końcowa jest wymagana.")]
        public DateTime PayoffTo { get; set; } = DateTime.Now;
    }
}
