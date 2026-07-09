using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace WaterCalculator.Common.Infrastructure.Cache
{
    public static class CacheExtensions
    {
        extension(IServiceCollection services)
        {
            /// <summary>
            /// Configures caching.
            /// </summary>
            public void ConfigureCache()
            {
                services.AddFusionCache()
                    .WithSerializer(new FusionCacheSystemTextJsonSerializer())
                    .WithDefaultEntryOptions(options =>
                    {
                        options.Duration = TimeSpan.FromMinutes(5);
                        options.IsFailSafeEnabled = true;
                        options.FailSafeMaxDuration = TimeSpan.FromMinutes(30);
                        options.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);
                    });

                services.AddSingleton<IAppCache, FusionAppCache>();
            }
        }
    }
}
