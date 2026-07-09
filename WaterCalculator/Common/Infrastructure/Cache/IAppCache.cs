namespace WaterCalculator.Common.Infrastructure.Cache
{
    public interface IAppCache
    {
        ValueTask<T> GetOrSetAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? duration = null,
            string[]? tags = null,
            CancellationToken ct = default);

        Task RemoveByTagAsync(string tag);
        Task RemoveKeyAsync(string key);
    }
}
