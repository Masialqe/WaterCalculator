namespace WaterCalculator.Domain
{
    public sealed class Invoice : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public decimal TotalConsumption { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public ICollection<Settlement> Settlements { get; set; } = [];
    }
}
