﻿namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    using Newtonsoft.Json;

    public record QuotePayload
    {
        public decimal Price { get; set; }

        [JsonProperty("last_updated")]
        public string? LastUpdated { get; set; }
    }
}
