using System.ComponentModel.DataAnnotations;

namespace WaterCalculator.Domain.Invoices
{
    public enum CalculationType
    {
        [Display(Name = "Na podstawie zużycia")]
        PerConsumption,

        [Display(Name = "Równy podział")]
        EqualSplit,

        [Display(Name = "Nie uwzględniaj w rozliczeniu")]
        NotIncluded
    }
}
