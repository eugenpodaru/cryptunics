namespace Cryptunics.Infrastructure.Repository
{
    using System;
    using System.Threading.Tasks;
    using Client.CoinMarketCap;
    using Core.Domain;
    using Core.Repository;
    using LazyCache;

    public class CoinMarketCapCryptoCoinQuoteRepository : CacheEnabledRepository, ICryptoCoinQuoteRepository
    {
        private readonly ICoinMarketCapClient _client;

        public CoinMarketCapCryptoCoinQuoteRepository(ICoinMarketCapClient client, IAppCache cache, CryptoCoinQuoteRepositoryOptions options) : base(cache, options)
            => _client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<Quote> GetLatestQuoteAsync(CryptoCoin @base, FiatCoin currency)
        {
            return await GetOrAddAsync(GetCacheKey(@base, currency), GetLatestListingsAsync);

            async Task<Quote> GetLatestListingsAsync()
            {
                var latestListings = await _client.GetLatestListingsAsync(currency, @base);
                var latestListing = latestListings.Data!.Values.Single();

                return latestListing.ToQuote(currency);
            }

            static string GetCacheKey(CryptoCoin @base, FiatCoin currency) => $"{nameof(CoinMarketCapCryptoCoinQuoteRepository)}_{@base.Id}_{currency.Id}";
        }
    }
}
