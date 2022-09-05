namespace Cryptunics.Infrastructure.Repository
{
    public record CacheOptions
    {
        public int AbsoluteCacheItemExpirationInMinutes { get; init; }

        public int SlidingWindowCacheItemExpirationInMinutes { get; init; }
    }

    public record CoinRepositoryOptions : CacheOptions;

    public record CryptoCoinQuoteRepositoryOptions : CacheOptions;

    public record FiatCoinQuoteRepositoryOptions : CacheOptions;
}
