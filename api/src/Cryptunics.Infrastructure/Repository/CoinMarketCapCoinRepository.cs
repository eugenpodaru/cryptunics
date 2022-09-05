namespace Cryptunics.Infrastructure.Repository
{
    using Client.CoinMarketCap;
    using Core.Domain;
    using Core.Repository;
    using LazyCache;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CoinMarketCapCoinRepository : CacheEnabledRepository, ICoinRepository
    {
        private readonly string _cryptoCoinsCacheKey = $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetCryptoCoinsAsync)}";
        private readonly string _fiatCoinsCacheKey = $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetFiatCoinsAsync)}";

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

        public Task<CryptoCoin> GetCryptoCoinByIdAsync(int id)
        {
            return GetOrAddAsync(GetCacheKey(id), AddAndGetCryptoCoinById);

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

                return searchCoin ?? throw new KeyNotFoundException();
            }

            static string GetCacheKey(int id) => $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetCryptoCoinByIdAsync)}_{id}";
        }

        public Task<FiatCoin> GetFiatCoinBySymbolAsync(string symbol)
        {
            return GetOrAddAsync(GetCacheKey(symbol), AddAndGetFiatCoinBySymbol);

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

                return searchCoin ?? throw new KeyNotFoundException();
            }

            static string GetCacheKey(string symbol) => $"{nameof(CoinMarketCapCoinRepository)}_{nameof(GetFiatCoinBySymbolAsync)}_{symbol}";
        }

        private Task<List<CryptoCoin>> GetCryptoCoinsAsync()
        {
            return GetOrAddAsync(_cryptoCoinsCacheKey, GetCryptoCoinMapAsync);

            async Task<List<CryptoCoin>> GetCryptoCoinMapAsync()
            {
                var coinMap = await _client.GetCryptoCoinMapAsync();

                return coinMap.Data!.Select(p => p.ToCryptoCoin()).ToList();
            }
        }

        private Task<List<FiatCoin>> GetFiatCoinsAsync()
        {
            return GetOrAddAsync(_fiatCoinsCacheKey, GetFiatCoinMapAsync);

            async Task<List<FiatCoin>> GetFiatCoinMapAsync()
            {
                var coinMap = await _client.GetFiatCoinMapAsync();

                return coinMap.Data!.Select(p => p.ToFiatCoin()).ToList();
            }
        }
    }
}
