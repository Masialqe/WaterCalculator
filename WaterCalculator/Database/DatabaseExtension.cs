using Microsoft.EntityFrameworkCore;

namespace WaterCalculator.Database
{
    public static class DatabaseExtension
    {
        extension(IServiceCollection services)
        {
            public void AddDatabase()
            {
                services.AddDbContextFactory<DatabaseContext>(options =>
                {
                    //For dev purposes, will be replaced by PostreSQL
                    options.UseSqlite("Data Source=water.db");
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
            }
        }

    }
}
