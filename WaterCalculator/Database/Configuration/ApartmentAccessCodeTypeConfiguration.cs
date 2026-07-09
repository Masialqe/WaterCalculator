using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain;

namespace WaterCalculator.Database.Configuration
{
    public class ApartmentAccessCodeTypeConfiguration : IEntityTypeConfiguration<ApartmentAccessCode>
    {
        public void Configure(EntityTypeBuilder<ApartmentAccessCode> builder)
        {
            builder.ToTable("apartment_access_code");
            builder.HasKey(acc => acc.Id);

            builder.Property(acc => acc.Id).HasColumnName("apartment_code_id");
            builder.Property(acc => acc.ApartmenId).HasColumnName("apartment_id");
            builder.Property(acc => acc.Code).HasMaxLength(255).HasColumnName("apartment_access_code");

            builder.Property(acc => acc.CreatedAt).HasColumnName("apartment_code_created_at");

            builder.HasIndex(acc => acc.ApartmenId);
        }
    }
}
