namespace Cryptunics.Core
{
    using Domain;
    using Repository;
    using System;

    public class CoinManager : ICoinManager
    {
        private readonly ICoinRepository _coinRepository;
        private readonly CoinOptions _options;

        private readonly Lazy<Task<FiatCoin>> _defaultFiatCoinLazy;
        private readonly Lazy<Task<FiatCoin[]>> _quoteFiatCoinsLazy;

        public CoinManager(ICoinRepository coinRepository, CoinOptions options)
        {
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _defaultFiatCoinLazy = new(() => _coinRepository.GetFiatCoinBySymbolAsync(_options.DefaultFiatCurrencySymbol));
            _quoteFiatCoinsLazy = new(() => GetFiatCoinsBySymbolsAsync(_options.QuoteFiatCurrencySymbols));
        }

        public Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync() => _coinRepository.GetAllFiatCoinsAsync();

        public Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync() => _coinRepository.GetAllCryptoCoinsAsync();

        public Task<CryptoCoin> GetCryptoCoinByIdAsync(int id) => _coinRepository.GetCryptoCoinByIdAsync(id);

        public Task<FiatCoin> GetDefaultFiatCoinAsync() => _defaultFiatCoinLazy.Value;

        public async Task<IEnumerable<FiatCoin>> GetQuoteFiatCoinsAsync() => await _quoteFiatCoinsLazy.Value;

        private Task<FiatCoin[]> GetFiatCoinsBySymbolsAsync(string[] symbols) => Task.WhenAll(symbols.Select(s => _coinRepository.GetFiatCoinBySymbolAsync(s)));
    }
}
