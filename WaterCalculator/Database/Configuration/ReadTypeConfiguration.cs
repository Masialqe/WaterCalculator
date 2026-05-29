using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain;

namespace WaterCalculator.Database.Configuration
{
    public class ReadTypeConfiguration : IEntityTypeConfiguration<Read>
    {
        public void Configure(EntityTypeBuilder<Read> builder)
        {
            builder.ToTable("reads");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnName("read_id");

            builder.Property(r => r.Amount).HasColumnName("amount").IsRequired();
            builder.Property(r => r.Value).HasColumnName("value").IsRequired();

            builder.HasIndex(r => r.ApartmentId);
            builder.HasIndex(r => r.CreatedAt);
        }
    }
}
