using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain.Reads;

namespace WaterCalculator.Database.Configuration
{
    public class ReadTypeConfiguration : IEntityTypeConfiguration<Read>
    {
        public void Configure(EntityTypeBuilder<Read> builder)
        {
            builder.ToTable("reads");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnName("read_id");
            
            builder.Property(r => r.Value).HasColumnName("value").IsRequired();
            builder.Property(r => r.ApartmentId).HasColumnName("apartment_id");
            builder.Property(r => r.ReadDate).HasColumnName("read_date");

            builder.HasIndex(r => r.ApartmentId);
            builder.HasIndex(r => r.CreatedAt);
            //builder.HasIndex(r => r.PayoffId);
        }
    }
}
