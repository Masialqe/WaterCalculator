using System.Globalization;
using System.Threading.RateLimiting;

namespace WaterCalculator.Common.Infrastructure.Limiters
{
    public static class RateLimitingExtensions
    {
        extension(IServiceCollection services)
        {
            //public void ConfigureRateLimiting()
            //{
            //    services.AddRateLimiter(options =>
            //    {
            //        options.OnRejected = async (context, cancellationToken) =>
            //        {
            //            context.HttpContext.Response.StatusCode =
            //                StatusCodes.Status429TooManyRequests;

            //            if (context.Lease.TryGetMetadata(
            //                    MetadataName.RetryAfter,
            //                    out var retryAfter))
            //            {
            //                context.HttpContext.Response.Headers.RetryAfter =
            //                    ((int)retryAfter.TotalSeconds)
            //                    .ToString(NumberFormatInfo.InvariantInfo);
            //            }

            //            await context.HttpContext.Response.WriteAsync(
            //                "Za dużo prób. Spróbuj później.",
            //                cancellationToken);
            //        };

            //        options.AddPolicy(RateLimitingPolicies.AdminLogin, httpContext =>
            //        {
            //            var clientKey = GetClientKey(httpContext);

            //            return RateLimitPartition.GetFixedWindowLimiter(
            //                partitionKey: $"admin-login:{clientKey}",
            //                factory: _ => new FixedWindowRateLimiterOptions
            //                {
            //                    PermitLimit = 5,
            //                    Window = TimeSpan.FromMinutes(15),
            //                    QueueLimit = 0,
            //                    AutoReplenishment = true
            //                });
            //        });

            //        options.AddPolicy(RateLimitingPolicies.ApartmentUnlock, httpContext =>
            //        {
            //            var clientKey = GetClientKey(httpContext);
            //            var token = httpContext.Request.RouteValues["token"]?.ToString()
            //                ?? "unknown";

            //            return RateLimitPartition.GetFixedWindowLimiter(
            //                partitionKey: $"apartment-unlock:{clientKey}:{token}",
            //                factory: _ => new FixedWindowRateLimiterOptions
            //                {
            //                    PermitLimit = 5,
            //                    Window = TimeSpan.FromMinutes(10),
            //                    QueueLimit = 0,
            //                    AutoReplenishment = true
            //                });
            //        });
            //    });
            //}
            public void ConfigureRateLimiting()
            {
                services.AddRateLimiter(options =>
                {
                    options.OnRejected = async (context, ct) =>
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.HttpContext.Response.WriteAsync("Try again later.");
                    };

                    options.AddPolicy("public-forms", httpContext =>
                    {
                        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey: $"login:{ip}",
                            factory: _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 5,
                                Window = TimeSpan.FromSeconds(30),
                                QueueLimit = 0,
                                AutoReplenishment = true
                            });

                    });
                });
            }
        }
    }
}
