using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.AccessControll;

namespace WaterCalculator.Database
{
    public static class DatabaseExtension
    {
        extension(IServiceCollection services)
        {
            public void AddDatabase()
            {
                var path = Path.Combine(AppContext.BaseDirectory, "water.db");

                services.AddDbContextFactory<DatabaseContext>(options =>
                {
                    options.UseSqlite($"Data Source={path}");
                });

                services.AddDbContext<IdentityContext>(options =>
                {
                    options.UseSqlite($"Data Source={path}");
                });
            }
        }

        extension(WebApplication app)
        {
            public void MigrateDatabase()
            {
                using var scope = app.Services.CreateScope();
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();

                using var dbContext = dbContextFactory.CreateDbContext();
                dbContext.Database.Migrate();

                var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                identityContext.Database.Migrate();
            }
        }
    }
}
