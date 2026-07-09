using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Domain.Invoices;
using WaterCalculator.Domain.Reads;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WaterCalculator.Domain
{
    public class Payoff : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public Guid? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        public PayoffStatus Status { get; set; } = PayoffStatus.Open;

        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }

        public ICollection<Read> Reads { get; set; } = [];
        public ICollection<Settlement> Settlements { get; set; } = [];
        public decimal TotalMeterValue { get; set; }
        public decimal TotalConsumptionValue { get; set; }

        [NotMapped]
        public int ReadsCount => Reads.Count;
        public Payoff() {}
        public Payoff(DateTime periodFrom, DateTime periodTo, Guid groupId)
        {
            PeriodFrom = periodFrom;
            PeriodTo = periodTo;
            GroupId = groupId;
        }

        public static Payoff Create(DateTime periodFrom, DateTime periodTo, Guid groupId)
             => new(periodFrom, periodTo, groupId);

        public Result Validate()
        {
            if (PeriodTo < PeriodFrom)
                return Errors.InvalidOperationError("Data końcowa musi byc późniejsza, niż startowa.");

            if(GroupId == Guid.Empty)
                return Errors.InvalidOperationError("Podana grupa jest nieprawidłowa.");

            return Result.Success();
        }
    
    }

    public enum PayoffStatus
    {
        [Display(Name = "Otwarte")]
        Open = 0,

        [Display(Name = "Podsumowane")]
        Summarized = 1,

        [Display(Name = "Nierozliczone")]
        Unsettled = 2,

        [Display(Name = "Rozliczone")]
        Settled = 3
    }
}
