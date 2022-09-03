namespace Cryptunics.Tests.Core
{
    using Cryptunics.Core;
    using Cryptunics.Core.Domain;
    using Cryptunics.Core.Repository;
    using FluentAssertions;
    using Moq;
    using System;
    using System.Threading.Tasks;
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
            var exchange = GetExchange(_data.Bitcoin, _data.UsdRate, Array.Empty<Rate>());

            var quote = await exchange.GetLatestQuoteAsync(_data.Bitcoin, _data.Usd);

            quote.Base.Should().Be(_data.Bitcoin);
            quote.Rates.Should().Equal(_data.UsdRate);
        }

        [Fact]
        public async Task Given_Exchange_GetLatestQuote_ReturnsDerivedRate()
        {
            var exchange = GetExchange(_data.Bitcoin, _data.UsdRate, _data.EuroRate);

            var quote = await exchange.GetLatestQuoteAsync(_data.Bitcoin, _data.Euro);

            quote.Base.Should().Be(_data.Bitcoin);
            quote.Rates.Should().Equal(_data.EuroRate with
            {
                IsDerived = true,
                Price = _data.UsdRate.Price * _data.EuroRate.Price,
                Timestamp = _data.Old
            });
        }

        private static IExchange GetExchange(CryptoCoin @base, Rate baseRate, params Rate[] rates)
        {
            var coinRepository = new Mock<ICoinRepository>();
            var cryptoCoinQuoteRepository = new Mock<ICryptoCoinQuoteRepository>();
            var fiatCoinQuoteRepository = new Mock<IFiatCoinQuoteRepository>();

            cryptoCoinQuoteRepository
                .Setup(r => r.GetLatestQuoteAsync(It.IsAny<CryptoCoin>(), It.IsAny<FiatCoin>()))
                .ReturnsAsync(() => new Quote(@base, new[] { baseRate }));
            fiatCoinQuoteRepository
                .Setup(r => r.GetLatestQuoteAsync(It.IsAny<FiatCoin>(), It.IsAny<FiatCoin[]>()))
                .ReturnsAsync(() => new Quote(baseRate.Currency, rates));

            return new Exchange(baseRate.Currency, coinRepository.Object, fiatCoinQuoteRepository.Object, cryptoCoinQuoteRepository.Object);
        }
    }
}
