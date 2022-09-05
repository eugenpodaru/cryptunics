namespace Cryptunics.Core
{
    using Domain;
    using Repository;
    using System;
    using System.Linq;

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

        public async Task<Quote> GetLatestQuoteAsync(int baseId)
        {
            var @base = await _coinManager.GetCryptoCoinByIdAsync(baseId);

            return await GetLatestQuoteAsync(@base);
        }

        public async Task<Quote> GetLatestQuoteAsync(CryptoCoin @base)
        {
            var defaultCurrency = await _coinManager.GetDefaultFiatCoinAsync();
            var quoteCurrencies = await _coinManager.GetQuoteFiatCoinsAsync();

            return quoteCurrencies.Count() switch
            {
                0 => Quote.Empty(@base),
                1 => quoteCurrencies.Single() == defaultCurrency ? await GetLatestQuoteForExchangeBaseAsync() : await GetLatestQuoteForCurrenciesAsync(),
                _ => await GetLatestQuoteForCurrenciesAsync()
            };

            Task<Quote> GetLatestQuoteForExchangeBaseAsync() => _cryptoCoinQuoteRepository.GetLatestQuoteAsync(@base, defaultCurrency);

            async Task<Quote> GetLatestQuoteForCurrenciesAsync()
            {
                var exchangeBaseQuote = await GetLatestQuoteForExchangeBaseAsync();
                var fiatQuote = await _fiatCoinQuoteRepository.GetLatestQuoteAsync(defaultCurrency, quoteCurrencies.Except(new[] { defaultCurrency }).ToArray());

                var exchangeBaseRate = exchangeBaseQuote.Rates.First(r => r.Currency == defaultCurrency);
                var fiatRates = fiatQuote.Rates.ToDictionary(r => r.Currency);

                var rates = quoteCurrencies.Select(c => GetDerivedRate(c, defaultCurrency, fiatRates[c], exchangeBaseRate)).ToArray();

                return exchangeBaseQuote with { Rates = rates };
            }
        }

        private static Rate GetDerivedRate(FiatCoin coin, FiatCoin @base, Rate coinRate, Rate baseRate)
        {
            if (coin == @base)
            {
                return baseRate;
            }

            var derivedPrice = coinRate.Price * baseRate.Price;
            var derivedTimestamp = coinRate.Timestamp < baseRate.Timestamp ? coinRate.Timestamp : baseRate.Timestamp;

            return coinRate with { Price = derivedPrice, IsDerived = true, Timestamp = derivedTimestamp };
        }
    }
}
