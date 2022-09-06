namespace Cryptunics.Core
{
    using System;
    using System.Linq;
    using Domain;
    using Repository;

    public sealed class Exchange : IExchange
    {
        private readonly ICoinManager _coinManager;
        private readonly IFiatCoinQuoteRepository _fiatCoinQuoteRepository;
        private readonly ICryptoCoinQuoteRepository _cryptoCoinQuoteRepository;

        public Exchange(ICoinManager coinManager, IFiatCoinQuoteRepository fiatCoinQuoteRepository, ICryptoCoinQuoteRepository cryptoCoinQuoteRepository)
        {
            _coinManager = coinManager ?? throw new ArgumentNullException(nameof(coinManager));
            _fiatCoinQuoteRepository = fiatCoinQuoteRepository ?? throw new ArgumentNullException(nameof(fiatCoinQuoteRepository));
            _cryptoCoinQuoteRepository = cryptoCoinQuoteRepository ?? throw new ArgumentNullException(nameof(cryptoCoinQuoteRepository));
        }

        public async Task<Quote> GetLatestQuoteAsync(int cryptoCoinId)
        {
            var @base = await _coinManager.GetCryptoCoinByIdAsync(cryptoCoinId);

            return await GetLatestQuoteAsync(@base);
        }

        public async Task<Quote> GetLatestQuoteAsync(CryptoCoin cryptoCoin)
        {
            var defaultCoin = await _coinManager.GetDefaultFiatCoinAsync();
            var quoteCoins = await _coinManager.GetQuoteFiatCoinsAsync();

            return quoteCoins.Count() switch
            {
                0 => Quote.Empty(cryptoCoin),
                1 => quoteCoins.Single() == defaultCoin ? await GetLatestQuoteForDefaultCoinAsync() : await GetLatestQuoteForCoinsAsync(),
                _ => await GetLatestQuoteForCoinsAsync()
            };

            Task<Quote> GetLatestQuoteForDefaultCoinAsync() => _cryptoCoinQuoteRepository.GetLatestQuoteAsync(cryptoCoin, defaultCoin);

            async Task<Quote> GetLatestQuoteForCoinsAsync()
            {
                var defaultCoinQuote = await GetLatestQuoteForDefaultCoinAsync();
                var coinsQuote = await _fiatCoinQuoteRepository.GetLatestQuoteAsync(defaultCoin, quoteCoins.Except(new[] { defaultCoin }).ToArray());

                var defaultCoinRate = defaultCoinQuote.Rates.First(r => r.Currency == defaultCoin);
                var coinsRates = coinsQuote.Rates.ToDictionary(r => r.Currency);

                var rates = quoteCoins.Select(coin =>
                {
                    if (coin == defaultCoin)
                    {
                        return defaultCoinRate;
                    }

                    var coinRate = coinsRates[coin];
                    var derivedPrice = coinRate.Price * defaultCoinRate.Price;
                    var derivedTimestamp = coinRate.Timestamp < defaultCoinRate.Timestamp ? coinRate.Timestamp : defaultCoinRate.Timestamp;

                    return coinRate with { Price = derivedPrice, IsDerived = true, Timestamp = derivedTimestamp };
                }).ToArray();

                return defaultCoinQuote with { Rates = rates };
            }
        }
    }
}
