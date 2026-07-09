using ZiggyCreatures.Caching.Fusion;

namespace WaterCalculator.Common.Infrastructure.Cache
{
    public class FusionAppCache(IFusionCache cache) : IAppCache
    {
        public ValueTask<T> GetOrSetAsync<T>(string key, 
            Func<CancellationToken, Task<T>> factory, 
            TimeSpan? duration = null, 
            string[]? tags = null, 
            CancellationToken ct = default)
        {
            return cache.GetOrSetAsync(key, 
                factory, 
                opt => 
                { 
                    if (duration.HasValue) 
                        opt.SetDuration(duration.Value); 
                }, 
                tags, 
                ct);
        }

        public async Task RemoveByTagAsync(string tag)
        {
            await cache.RemoveByTagAsync(tag);
        }

        public async Task RemoveKeyAsync(string key)
        {
            await cache.RemoveAsync(key);
        }
    }
}
