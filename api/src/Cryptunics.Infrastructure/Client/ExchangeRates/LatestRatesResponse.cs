namespace Cryptunics.Infrastructure.Client.ExchangeRates
{
    using System.Collections.Generic;

    public record LatestRatesResponse
    {
        public string? Base { get; init; }

        public string? Date { get; init; }

        public Dictionary<string, decimal>? Rates { get; init; }

        public bool Success { get; init; }

        public long Timestamp { get; init; }
    }
}
