namespace Cryptunics.Tests.Core
{
    using Cryptunics.Core.Domain;
    using System;

    public class ExchangeFixture
    {
        // timestamps
        public DateTimeOffset Old { get; }
        public DateTimeOffset New { get; }

        // coins
        public CryptoCoin Bitcoin { get; }
        public FiatCoin Usd { get; }
        public FiatCoin Euro { get; }
        public FiatCoin Pound { get; }

        // rates
        public Rate UsdRate { get; }
        public Rate EuroRate { get; }
        public Rate PoundRate { get; }

        public ExchangeFixture()
        {
            Old = DateTimeOffset.Now.AddMinutes(-1);
            New = DateTimeOffset.Now;

            Bitcoin = new(1, "BTC");
            Usd = new(2, "USD");
            Euro = new(3, "EUR");
            Pound = new(4, "GBP");

            UsdRate = new(Usd, 2, Old);
            EuroRate = new(Euro, 1, New);
            PoundRate = new(Pound, 2, Old);
        }
    }
}
