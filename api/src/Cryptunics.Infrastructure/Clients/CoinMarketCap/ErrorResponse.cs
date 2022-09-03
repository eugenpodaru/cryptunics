namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    public record ErrorResponse
    {
        public StatusPayload? Status { get; init; }
    }
}
