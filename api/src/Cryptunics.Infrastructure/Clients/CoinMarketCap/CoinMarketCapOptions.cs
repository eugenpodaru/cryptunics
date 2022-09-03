namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    public record CoinMarketCapOptions
    {
        public string? Url { get; init; }

        public string? Key { get; init; }
    }
}
