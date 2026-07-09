using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain;

namespace WaterCalculator.Database.Configuration
{
    public class AparmentTypeConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("apartments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("apartment_id");

            builder.Property(x => x.Name)
                .HasColumnName("apartment_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Details)
                .HasColumnName("apartment_details")
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt).HasColumnName("apartment_created_at");

            builder.Property(x => x.PublicToken)
                .HasMaxLength(255)
                .HasColumnName("apartment_public_token");

            builder.HasMany(x => x.Reads)
                .WithOne(x => x.Apartment)
                .HasForeignKey(x => x.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Settlements)
                .WithOne(x => x.Apartment)
                .HasForeignKey(x => x.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.AccessCode)
                .WithOne(ac => ac.Apartment)
                .HasForeignKey<ApartmentAccessCode>(ac => ac.ApartmenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name);
        }
    }
}
