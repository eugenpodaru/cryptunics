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

        public async Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync() => await _coinRepository.GetAllFiatCoinsAsync();

        public async Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync() => await _coinRepository.GetAllCryptoCoinsAsync();

        public async Task<CryptoCoin> GetCryptoCoinByIdAsync(int id) => await _coinRepository.GetCryptoCoinByIdAsync(id);

        public async Task<FiatCoin> GetDefaultFiatCoinAsync() => await _defaultFiatCoinLazy.Value;

        public async Task<IEnumerable<FiatCoin>> GetQuoteFiatCoinsAsync() => await _quoteFiatCoinsLazy.Value;

        private Task<FiatCoin[]> GetFiatCoinsBySymbolsAsync(string[] symbols) => Task.WhenAll(symbols.Select(s => _coinRepository.GetFiatCoinBySymbolAsync(s)));
    }
}
