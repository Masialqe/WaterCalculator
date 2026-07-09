using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Database.Configuration
{
    public class PayoffTypeConfiguration : IEntityTypeConfiguration<Payoff>
    {
        public void Configure(EntityTypeBuilder<Payoff> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("payoff_id");

            builder.Property(p => p.CreatedAt).HasColumnName("payoff_created_at");
            builder.Property(p => p.Status).HasColumnName("payoff_status");
            builder.Property(p => p.TotalMeterValue).HasColumnName("payoff_total_meter_value");
            builder.Property(p => p.PeriodTo).HasColumnName("payoff_period_to");
            builder.Property(p => p.PeriodTo).HasColumnName("payoff_period_to");
            builder.Property(p => p.TotalConsumptionValue).HasColumnName("payoff_total_consumption");

            builder.HasMany(p => p.Reads)
                .WithOne(r => r.Payoff)
                .HasForeignKey(r => r.PayoffId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Invoice)
                .WithOne(i => i.Payoff)
                .HasForeignKey<Invoice>(i => i.PayoffId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Group)
                .WithMany(g => g.Payoffs)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Settlements)
                .WithOne(s => s.Payoff)
                .HasForeignKey(s => s.PayoffId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.GroupId);
        }
    }
}
