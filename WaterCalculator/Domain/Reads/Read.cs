using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Domain.Reads
{
    public class Read : BaseEntity
    {
        public Guid ApartmentId {  get; set; }
        public Apartment Apartment { get; set; } = null!;
        public decimal Value { get; set; }
        public decimal ConsumptionFromLastRead { get; set; }
        public DateTime ReadDate { get; set; }

        public Guid? PayoffId { get; set; }
        public Payoff? Payoff { get; set; } = null!;

        public Read(){}

        public Read(decimal value, DateTime readDate, Guid apartmentId, Guid? payoffId)
        {
            Value = value;
            ApartmentId = apartmentId;
            ReadDate = readDate;
            PayoffId = payoffId;
        }
        public static Read Create(decimal value, DateTime readDate, Guid apartmentId, Guid? payoffId)
            => new Read(value, readDate, apartmentId, payoffId);

        public Result Validate(bool isFirstRead)
        {
            if(!isFirstRead && (PayoffId is null || PayoffId == Guid.Empty))
                return Errors.InvalidOperationError("Odczyt musi być przypisany do rozliczenia.");

            if(!isFirstRead && Value == 0)
                return Errors.InvalidOperationError("Odczyt kolejny nie może wynosić 0.");

            if (ApartmentId == Guid.Empty)
                return Errors.InvalidOperationError("Odczyt musi być przypisany do mieszkania.");

            if(ReadDate > DateTime.Now)
                return Errors.InvalidOperationError("Odczyt nie może być z przyszłości.");

            if(Value < 0)
                return Errors.InvalidOperationError("Wartość odczytu nie może być mniejsza niż 0.");

            return Result.Success();
        }
    }
}
