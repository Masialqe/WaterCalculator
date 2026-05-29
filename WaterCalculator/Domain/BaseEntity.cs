namespace WaterCalculator.Domain
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
