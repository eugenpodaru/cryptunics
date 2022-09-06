namespace Cryptunics.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client.CoinMarketCap;
    using Core.Domain;
    using Core.Repository;
    using LazyCache;

    public class CoinMarketCapCoinRepository : CacheEnabledRepository, ICoinRepository
    {
        private readonly ICoinMarketCapClient _client;

        private readonly Lazy<Task<List<CryptoCoin>>> _cryptoCoinsLazy;
        private readonly Lazy<Task<List<FiatCoin>>> _fiatCoinsLazy;

        public CoinMarketCapCoinRepository(ICoinMarketCapClient client, IAppCache cache, CoinRepositoryOptions options) : base(cache, options)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));

            _cryptoCoinsLazy = new(() => GetCryptoCoinsAsync());
            _fiatCoinsLazy = new(() => GetFiatCoinsAsync());
        }

        public async Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync() => await _cryptoCoinsLazy.Value;

        public async Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync() => await _fiatCoinsLazy.Value;

        public async Task<CryptoCoin> GetCryptoCoinByIdAsync(int id)
        {
            return await GetOrAddAsync(GetCacheKey(id), AddAndGetCryptoCoinById);

            async Task<CryptoCoin> AddAndGetCryptoCoinById()
            {
                var searchCoin = default(CryptoCoin);
                var coins = await GetAllCryptoCoinsAsync();

                foreach (var coin in coins)
                {
                    if (coin.Id == id)
                    {
                        searchCoin = coin;
                    }

                    Add(GetCacheKey(coin.Id), coin);
                }

                return searchCoin ?? throw new KeyNotFoundException($"Could not find crypto coin for id: {id}.");
            }

            static string GetCacheKey(int id) => $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetCryptoCoinByIdAsync)}_{id}";
        }

        public async Task<FiatCoin> GetFiatCoinBySymbolAsync(string symbol)
        {
            return await GetOrAddAsync(GetCacheKey(symbol), AddAndGetFiatCoinBySymbol);

            async Task<FiatCoin> AddAndGetFiatCoinBySymbol()
            {
                var searchCoin = default(FiatCoin);
                var coins = await GetAllFiatCoinsAsync();

                foreach (var coin in coins)
                {
                    if (coin.Symbol == symbol)
                    {
                        searchCoin = coin;
                    }

                    Add(GetCacheKey(coin.Symbol), coin);
                }

                return searchCoin ?? throw new KeyNotFoundException($"Could not find fiat coin for symbol: {symbol}.");
            }

            static string GetCacheKey(string symbol) => $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetFiatCoinBySymbolAsync)}_{symbol}";
        }

        private Task<List<CryptoCoin>> GetCryptoCoinsAsync()
        {
            return GetOrAddAsync(GetCacheKey(), GetCryptoCoinMapAsync);

            async Task<List<CryptoCoin>> GetCryptoCoinMapAsync()
            {
                var coinMap = await _client.GetCryptoCoinMapAsync();

                return coinMap.Data!.Select(p => p.ToCryptoCoin()).ToList();
            }

            static string GetCacheKey() => $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetCryptoCoinsAsync)}";
        }

        private Task<List<FiatCoin>> GetFiatCoinsAsync()
        {
            return GetOrAddAsync(GetCacheKey(), GetFiatCoinMapAsync);

            async Task<List<FiatCoin>> GetFiatCoinMapAsync()
            {
                var coinMap = await _client.GetFiatCoinMapAsync();

                return coinMap.Data!.Select(p => p.ToFiatCoin()).ToList();
            }

            static string GetCacheKey() => $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetFiatCoinsAsync)}";
        }
    }
}
