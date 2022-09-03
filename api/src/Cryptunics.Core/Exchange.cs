namespace Cryptunics.Core
{
    using Domain;
    using Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Exchange : IExchange
    {
        private readonly FiatCoin _exchangeBase;
        private readonly ICoinRepository _coinRepository;
        private readonly IFiatCoinQuoteRepository _fiatCoinQuoteRepository;
        private readonly ICryptoCoinQuoteRepository _cryptoCoinQuoteRepository;

        public Exchange(FiatCoin exchangeBase, ICoinRepository coinRepository, IFiatCoinQuoteRepository fiatCoinQuoteRepository, ICryptoCoinQuoteRepository cryptoCoinQuoteRepository)
        {
            _exchangeBase = exchangeBase ?? throw new ArgumentNullException(nameof(exchangeBase));
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _fiatCoinQuoteRepository = fiatCoinQuoteRepository ?? throw new ArgumentNullException(nameof(fiatCoinQuoteRepository));
            _cryptoCoinQuoteRepository = cryptoCoinQuoteRepository ?? throw new ArgumentNullException(nameof(cryptoCoinQuoteRepository));
        }

        public Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync() => _coinRepository.GetAllFiatCoinsAsync();

        public Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync() => _coinRepository.GetAllCryptoCoinsAsync();

        public Task<Quote> GetLatestQuoteAsync(CryptoCoin @base, params FiatCoin[] currencies)
        {
            return currencies.Length switch
            {
                0 => Task.FromResult(Quote.Empty(@base)),
                1 => currencies.Single() == _exchangeBase ? GetLatestQuoteForExchangeBaseAsync() : GetLatestQuoteForCurrenciesAsync(),
                _ => GetLatestQuoteForCurrenciesAsync()
            };

            Task<Quote> GetLatestQuoteForExchangeBaseAsync() => _cryptoCoinQuoteRepository.GetLatestQuoteAsync(@base, _exchangeBase);

            async Task<Quote> GetLatestQuoteForCurrenciesAsync()
            {
                var exchangeBaseQuote = await GetLatestQuoteForExchangeBaseAsync();
                var fiatQuote = await _fiatCoinQuoteRepository.GetLatestQuoteAsync(_exchangeBase, currencies.Except(new[] { _exchangeBase }).ToArray());

                var exchangeBaseRate = exchangeBaseQuote.Rates.First(r => r.Currency == _exchangeBase);
                var fiatRates = fiatQuote.Rates.ToDictionary(r => r.Currency);

                var rates = currencies.Select(c => GetDerivedRate(c, _exchangeBase, fiatRates[c], exchangeBaseRate)).ToArray();

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
