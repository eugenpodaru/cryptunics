namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    public record ResponseV1<T>
    {
        public T[]? Data { get; init; }

        public StatusPayload? Status { get; init; }
    }
}
