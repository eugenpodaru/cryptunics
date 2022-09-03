namespace Cryptunics.Tests.Infrastructure
{
    using Cryptunics.Core.Domain;
    using Cryptunics.Infrastructure.Client.CoinMarketCap;

    public class CoinMarketCapClientFixture
    {
        public CoinMarketCapOptions Options { get; }

        public FiatCoin Usd { get; }

        public CryptoCoin Random { get; }

        public CoinMarketCapClientFixture()
        {
            Options = new CoinMarketCapOptions
            {
                Key = "b54bcf4d-1bca-4e8e-9a24-22ff2c3d462c",
                Url = "https://sandbox-api.coinmarketcap.com"
            };

            Usd = new(2781, "USD");
            Random = new(8886, "8qp7t5j1mq3");
        }
    }
}
