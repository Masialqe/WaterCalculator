using System.ComponentModel.DataAnnotations;

namespace WaterCalculator.Features.Apartments.Create;

public sealed class CreateApartmentFormModel
{
    [Required]
    [MaxLength(100, ErrorMessage = "Apartment name cannot be longer than 100 characters.")]
    [MinLength(2, ErrorMessage = "Apartment name cannot be smaller than 2 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9 .\-\/]+$", ErrorMessage =  "Apartment name can only contain letters, numbers, and dashes")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500, ErrorMessage = "Apartment number cannot be longer than 500 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage =  "Apartment name can only contain letters and numbers.")]
    public string Description { get; set; } = string.Empty;
    
    public Guid? GroupId { get; set; }
}