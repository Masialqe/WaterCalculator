using Microsoft.AspNetCore.Identity;

namespace WaterCalculator.Common.Infrastructure.AccessControll
{
    public static class IdentityExtensions
    {
        extension(IServiceCollection services)
        {
            //DEVONLY
            public void ConfigureDevIdentity()
            {
                services.AddCascadingAuthenticationState();
                services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;

                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                        .AddEntityFrameworkStores<IdentityContext>();

                services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.AccessDeniedPath = "/access-denied";
                });

                services.AddAuthorization();
            }
            public void ConfigureIdentity()
            {
                services.AddCascadingAuthenticationState();
                services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>();

                services.AddAuthentication(options =>
                {

                });

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("AdminOnly", policy =>
                    {
                        policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole("Admin");
                    });
                });

                services.AddAntiforgery();
            }

            public void ConfigureCookies()
            {
                services.ConfigureApplicationCookie(options => 
                {
                    options.Cookie.Name = "_Host-AdminIdentity";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.Path = "/";

                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                });
            }
        }

        extension(WebApplication app)
        {
            public void UseIdentity()
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
        }
    }
}
