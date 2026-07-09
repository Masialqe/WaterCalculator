using System.ComponentModel.DataAnnotations;
using WaterCalculator.Components.Shared.Validators;

namespace WaterCalculator.Components.Apartments;

public sealed class ApartmentFormModel
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [MaxLength(100, ErrorMessage = "Apartment name cannot be longer than 100 characters.")]
    [MinLength(2, ErrorMessage = "Apartment name cannot be smaller than 2 characters.")]
    [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ0-9 .\-\/]+$", ErrorMessage = "Apartment name can only contain letters, numbers, spaces, dots, dashes and slashes.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Apartment number cannot be longer than 500 characters.")]
    [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ0-9 ]*$", ErrorMessage = "Apartment description can only contain letters, numbers and spaces.")]
    public string Description { get; set; } = string.Empty;

    [NotEmptyGuid(ErrorMessage = "Grupa jest wymagana.")]
    public Guid GroupId { get; set; } = Guid.Empty;
}