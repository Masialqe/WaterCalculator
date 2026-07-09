using System.ComponentModel.DataAnnotations;

namespace WaterCalculator.Components.Groups
{
    public class GroupFormModel
    {
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [MaxLength(100, ErrorMessage = "Nazwa może mieć maksymalnie 100 znaków.")]
        [MinLength(2, ErrorMessage = "Nazwa musi mieć minimum 2 znaki.")]
        public string GroupName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Opis może mieć maksymalnie 100 znaków.")]
        public string GroupDetails { get; set; } = string.Empty;
    }
}
