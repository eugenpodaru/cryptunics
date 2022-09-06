namespace Cryptunics.Tests.Infrastructure
{
    using System.Threading.Tasks;
    using Cryptunics.Infrastructure.Client.CoinMarketCap;
    using FluentAssertions;
    using Xunit;

    public class CoinMarketCapClientTests : IClassFixture<CoinMarketCapClientFixture>
    {
        private readonly CoinMarketCapClientFixture _data;

        public CoinMarketCapClientTests(CoinMarketCapClientFixture data) => _data = data;

        [Fact(Skip = "Does an http call; Used only to validate response parsing.")]
        public async Task Given_CoinMarketCapClient_GetFiatCoinMap_ReturnsSuccessfully()
        {
            var client = new CoinMarketCapClient(_data.Options);

            var coinMapResponse = await client.GetFiatCoinMapAsync();

            coinMapResponse.Status?.ErrorCode.Should().Be(0);
            coinMapResponse.Data?.Should().HaveCountGreaterThan(0);
        }

        [Fact(Skip = "Does an http call; Used only to validate response parsing.")]
        public async Task Given_CoinMarketCapClient_GetCryptoCoinMap_ReturnsSuccessfully()
        {
            var client = new CoinMarketCapClient(_data.Options);

            var coinMapResponse = await client.GetCryptoCoinMapAsync();

            coinMapResponse.Status?.ErrorCode.Should().Be(0);
            coinMapResponse.Data?.Should().HaveCountGreaterThan(0);
        }

        [Fact(Skip = "Does an http call; Used only to validate response parsing.")]
        public async Task Given_CoinMarketCapClient_GetLatestListings_ReturnsSuccessfully()
        {
            var client = new CoinMarketCapClient(_data.Options);

            var coinMapResponse = await client.GetLatestListingsAsync(_data.Usd, _data.Random);

            coinMapResponse.Status?.ErrorCode.Should().Be(0);
            coinMapResponse.Data?.Should().HaveCount(1);
        }
    }
}
