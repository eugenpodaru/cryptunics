namespace Cryptunics.Infrastructure.Client.CoinMarketCap
{
    using System.Collections.Generic;

    public record ResponseV2<T>
    {
        public Dictionary<string, T>? Data { get; init; }

        public StatusPayload? Status { get; init; }
    }
}
