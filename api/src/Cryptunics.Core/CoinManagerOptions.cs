namespace Cryptunics.Core
{
    public record CoinManagerOptions
    {
        public string DefaultFiatCurrencySymbol { get; init; } = "USD";

        public string[] QuoteFiatCurrencySymbols { get; init; } = Array.Empty<string>();
    }
}
