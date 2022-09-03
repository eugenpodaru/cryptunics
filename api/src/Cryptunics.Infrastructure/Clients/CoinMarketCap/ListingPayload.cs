namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public record ListingPayload
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? Symbol { get; init; }

        public string? Slug { get; init; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; init; }

        public Dictionary<string, QuotePayload>? Quote { get; init; }
    }
}
