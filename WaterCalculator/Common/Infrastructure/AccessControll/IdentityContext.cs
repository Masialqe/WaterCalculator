using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WaterCalculator.Common.Infrastructure.AccessControll
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
    }
}
