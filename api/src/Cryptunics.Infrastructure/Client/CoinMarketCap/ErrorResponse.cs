namespace Cryptunics.Infrastructure.Client.CoinMarketCap
{
    public record ErrorResponse
    {
        public StatusPayload? Status { get; init; }
    }
}
