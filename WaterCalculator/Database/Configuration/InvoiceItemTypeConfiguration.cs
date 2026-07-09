using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Database.Configuration
{
    public class InvoiceItemTypeConfiguration : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            builder.ToTable("invoice_items");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnName("ii_id");

            builder.Property(i => i.Name)
                .HasColumnName("ii_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(i => i.Amount).HasColumnName("ii_amount");
            builder.Property(i => i.PricePerUnit).HasColumnName("ii_price_per_unit");
            builder.Property(i => i.BruttoPricePerUnit).HasColumnName("ii_bruttoprice_per_unit");
            builder.Property(i => i.Vat).HasColumnName("ii_vat");
            builder.Property(i => i.CreatedAt).HasColumnName("ii_created_at");
            builder.Property(i => i.CalculationType).HasColumnType("ii_calculation_type").HasConversion<int>();
            builder.Property(i => i.InvoiceId).HasColumnName("ii_invoice_id");
            builder.Property(i => i.TotalNettoPrice).HasColumnName("ii_total_netto_price");
            builder.Property(i => i.TotalBruttoPrice).HasColumnName("ii_total_brutto_price");

            builder.HasOne(i => i.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(i => i.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(i => i.InvoiceId);
        }
    }
}
