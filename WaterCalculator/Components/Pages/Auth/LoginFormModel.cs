using System.ComponentModel.DataAnnotations;

namespace WaterCalculator.Components.Pages.Auth
{
    internal sealed class LoginFormModel
    {
        [Required(ErrorMessage = "Podaj email.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny email.")]
        [StringLength(256, ErrorMessage = "Email jest za długi.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj hasło.")]
        [StringLength(128, ErrorMessage = "Hasło jest za długie.")]
        public string Password { get; set; } = string.Empty;

        [StringLength(0, ErrorMessage = "Nieprawidłowy formularz.")]
        public string RepeatPassword { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
