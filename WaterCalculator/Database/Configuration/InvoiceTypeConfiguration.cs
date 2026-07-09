using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Database.Configuration
{
    public class InvoiceTypeConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("invoices");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnName("invoice_id");

            builder.Property(i => i.Name)
                .HasColumnName("invoice_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.Number)
                .HasColumnName("invoice_number")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.TotalPrice).HasColumnName("invoice_total_price");
            builder.Property(i => i.TotalConsumption).HasColumnName("invoice_total_consumption");

            builder.Property(i => i.InvoiceDate).HasColumnName("invoice_date");


            builder.HasMany(i => i.Settlements)
                    .WithOne(s => s.Invoice)
                    .HasForeignKey(s => s.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasIndex(i => i.PayoffId);
        }
    }
}
