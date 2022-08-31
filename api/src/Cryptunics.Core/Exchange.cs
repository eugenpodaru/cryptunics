namespace Cryptunics.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Exchange
    {
        private readonly FiatCoin _base;
        private readonly ICoinRepository _coinRepository;
        private readonly IFiatCoinQuoteRepository _fiatCoinQuoteRepository;
        private readonly ICryptoCoinQuoteRepository _cryptoCoinQuoteRepository;

        public Exchange(FiatCoin @base, ICoinRepository coinRepository, IFiatCoinQuoteRepository fiatCoinQuoteRepository, ICryptoCoinQuoteRepository cryptoCoinQuoteRepository)
        {
            _base = @base ?? throw new ArgumentNullException(nameof(@base));
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _fiatCoinQuoteRepository = fiatCoinQuoteRepository ?? throw new ArgumentNullException(nameof(fiatCoinQuoteRepository));
            _cryptoCoinQuoteRepository = cryptoCoinQuoteRepository ?? throw new ArgumentNullException(nameof(cryptoCoinQuoteRepository));
        }

        public IEnumerable<FiatCoin> GetAllFiatCoins() => _coinRepository.GetAllFiatCoins();

        public IEnumerable<CryptoCoin> GetAllCryptoCoins() => _coinRepository.GetAllCryptoCoins();

        public FiatCoin GetFiatCoinById(int id) => _coinRepository.GetFiatCoinById(id);

        public FiatCoin GetFiatCoinBySymbol(string symbol) => _coinRepository.GetFiatCoinBySymbol(symbol);

        public CryptoCoin GetCryptoCoinById(int id) => _coinRepository.GetCryptoCoinById(id);

        public Quote GetLatestQuote(CryptoCoin @base, IEnumerable<FiatCoin> currencies)
        {
            var cryptoQuote = _cryptoCoinQuoteRepository.GetLatestQuote(@base, _base);
            var fiatQuote = _fiatCoinQuoteRepository.GetLatestQuote(_base, currencies.Except(new[] { _base }));

            var baseRate = cryptoQuote.Rates.First(r => r.Currency == _base);
            var fiatRates = fiatQuote.Rates.ToDictionary(r => r.Currency);

            var rates = currencies.Select(c => GetDerivedRate(c, _base, fiatRates[c], baseRate));

            return cryptoQuote with { Rates = rates };
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
