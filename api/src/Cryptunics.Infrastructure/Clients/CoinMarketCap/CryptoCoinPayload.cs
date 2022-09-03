namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    public record CryptoCoinPayload
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? Symbol { get; init; }

        public string? Slug { get; init; }
    }
}
