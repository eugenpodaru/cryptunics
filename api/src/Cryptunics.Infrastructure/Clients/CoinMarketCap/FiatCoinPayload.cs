namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    public record FiatCoinPayload
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? Sign { get; init; }

        public string? Symbol { get; init; }
    }
}
