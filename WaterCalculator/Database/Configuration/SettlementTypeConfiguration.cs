using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain;

namespace WaterCalculator.Database.Configuration
{
    public class SettlementTypeConfiguration : IEntityTypeConfiguration<Settlement>
    {
        public void Configure(EntityTypeBuilder<Settlement> builder)
        {
            builder.ToTable("settlements");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("settlement_id");

            builder.Property(s => s.ApartmentId).HasColumnName("apartment_id");
            builder.Property(s => s.InvoiceId).HasColumnName("invoice_id");

            builder.Property(s => s.Consumption).HasColumnName("consumption");
            builder.Property(s => s.AmountToPay).HasColumnName("amount_to_pay");

            builder.Property(s => s.RealizationStatus).HasColumnName("realization_status").HasConversion<int>();

            builder.HasIndex(s => s.ApartmentId);
            builder.HasIndex(s => s.InvoiceId);
            builder.HasIndex(s => s.CreatedAt);
            builder.HasIndex(s => s.PayoffId);
        }
    }
}