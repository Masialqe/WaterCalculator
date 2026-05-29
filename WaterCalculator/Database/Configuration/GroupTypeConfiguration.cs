using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaterCalculator.Domain;


namespace WaterCalculator.Database.Configuration
{
    public class GroupTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("groups");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("group_id");

            builder.Property(x => x.Name)
                .HasColumnName("group_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Details)
                .HasColumnName("group_details")
                .HasMaxLength(500);

            builder.HasMany(x => x.Apartments)
                .WithOne(a => a.Group)
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
