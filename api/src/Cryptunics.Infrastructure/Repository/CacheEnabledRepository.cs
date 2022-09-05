namespace Cryptunics.Infrastructure.Repository
{
    using LazyCache;
    using Microsoft.Extensions.Caching.Memory;

    public abstract class CacheEnabledRepository
    {
        private readonly IAppCache _cache;
        private readonly CacheOptions _options;

        private readonly Lazy<MemoryCacheEntryOptions> _cacheEntryOptionsLazy;

        public MemoryCacheEntryOptions CacheEntryOptions => _cacheEntryOptionsLazy.Value;

        public CacheEnabledRepository(IAppCache cache, CacheOptions options)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _cacheEntryOptionsLazy = new(() => GetCacheEntryOptions());
        }

        protected Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory) => _cache.GetOrAddAsync(key, addItemFactory, CacheEntryOptions);

        protected void Add<T>(string key, T item) => _cache.Add(key, item, CacheEntryOptions);

        private MemoryCacheEntryOptions GetCacheEntryOptions() => new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.AbsoluteCacheItemExpirationInMinutes),
            SlidingExpiration = TimeSpan.FromMinutes(_options.SlidingWindowCacheItemExpirationInMinutes)
        };
    }
}
