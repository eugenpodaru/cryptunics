namespace Cryptunics.Infrastructure.Repository
{
    using System;
    using System.Threading.Tasks;
    using Client.ExchangeRates;
    using Core.Domain;
    using Core.Repository;
    using LazyCache;

    public class ExchangeRatesFiatCoinQuoteRepository : CacheEnabledRepository, IFiatCoinQuoteRepository
    {
        private readonly IExchangeRatesClient _client;

        public ExchangeRatesFiatCoinQuoteRepository(IExchangeRatesClient client, IAppCache cache, FiatCoinQuoteRepositoryOptions options) : base(cache, options)
            => _client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<Quote> GetLatestQuoteAsync(FiatCoin @base, params FiatCoin[] currencies)
        {
            return await GetOrAddAsync(GetCacheKey(@base, currencies), GetLatestRatesAsync);

            async Task<Quote> GetLatestRatesAsync()
            {
                var latestRates = await _client.GetLatestRatesAsync(@base, currencies);

                return latestRates.ToQuote(@base, currencies);
            }

            static string GetCacheKey(FiatCoin @base, FiatCoin[] currencies) => $"{nameof(ExchangeRatesFiatCoinQuoteRepository)}_{@base.Id}_{string.Join(",", currencies.Select(c => c.Id).OrderBy(id => id))}";
        }
    }
}
