namespace Cryptunics.Core
{
    public record CoinOptions
    {
        public string DefaultFiatCurrencySymbol { get; init; } = "USD";

        public string[] QuoteFiatCurrencySymbols { get; init; } = new[] { "EUR", "BRL", "GBP", "AUD" };
    }
}
