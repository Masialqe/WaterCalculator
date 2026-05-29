using Microsoft.EntityFrameworkCore;
using WaterCalculator.Domain;

namespace WaterCalculator.Database
{
    public sealed class DatabaseContext : DbContext
    {
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Read> Reads { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);   
        }
    }
}
