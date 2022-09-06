namespace Cryptunics.Tests.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cryptunics.Core;
    using Cryptunics.Core.Domain;
    using Cryptunics.Core.Repository;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ExchangeTests : IClassFixture<ExchangeFixture>
    {
        private readonly ExchangeFixture _data;

        public ExchangeTests(ExchangeFixture data) => _data = data;

        [Fact]
        public async Task Given_Exchange_GetLatestQuote_ReturnsEmpty()
        {
            var exchange = GetExchange(_data.Bitcoin, _data.UsdRate, Array.Empty<Rate>());

            var quote = await exchange.GetLatestQuoteAsync(_data.Bitcoin);

            quote.Base.Should().Be(_data.Bitcoin);
            quote.Rates.Should().BeEmpty();
        }

        [Fact]
        public async Task Given_Exchange_GetLatestQuote_ReturnsBaseRate()
        {
            var exchange = GetExchange(_data.Bitcoin, _data.UsdRate, _data.UsdRate);

            var quote = await exchange.GetLatestQuoteAsync(_data.Bitcoin);

            quote.Base.Should().Be(_data.Bitcoin);
            quote.Rates.Should().Equal(_data.UsdRate);
        }

        [Fact]
        public async Task Given_Exchange_GetLatestQuote_ReturnsDerivedRate()
        {
            var exchange = GetExchange(_data.Bitcoin, _data.UsdRate, _data.EuroRate);

            var quote = await exchange.GetLatestQuoteAsync(_data.Bitcoin);

            quote.Base.Should().Be(_data.Bitcoin);
            quote.Rates.Should().Equal(_data.EuroRate with
            {
                IsDerived = true,
                Price = _data.UsdRate.Price * _data.EuroRate.Price,
                Timestamp = _data.Old
            });
        }

        private static IExchange GetExchange(CryptoCoin cryptoCoin, Rate defaultRate, params Rate[] rates)
        {
            var coinManager = new Mock<ICoinManager>();
            var cryptoCoinQuoteRepository = new Mock<ICryptoCoinQuoteRepository>();
            var fiatCoinQuoteRepository = new Mock<IFiatCoinQuoteRepository>();

            coinManager
                .Setup(m => m.GetDefaultFiatCoinAsync())
                .ReturnsAsync(() => defaultRate.Currency);
            coinManager
                .Setup(m => m.GetQuoteFiatCoinsAsync())
                .ReturnsAsync(() => rates.Select(r => r.Currency));
            cryptoCoinQuoteRepository
                .Setup(r => r.GetLatestQuoteAsync(It.IsAny<CryptoCoin>(), It.IsAny<FiatCoin>()))
                .ReturnsAsync(() => new Quote(cryptoCoin, new[] { defaultRate }));
            fiatCoinQuoteRepository
                .Setup(r => r.GetLatestQuoteAsync(It.IsAny<FiatCoin>(), It.IsAny<FiatCoin[]>()))
                .ReturnsAsync(() => new Quote(defaultRate.Currency, rates));

            return new Exchange(coinManager.Object, fiatCoinQuoteRepository.Object, cryptoCoinQuoteRepository.Object);
        }
    }
}
