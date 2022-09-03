namespace Cryptunics.Infrastructure.Client.CoinMarketCap
{
    using Newtonsoft.Json;

    public record StatusPayload
    {
        public string? Timestamp { get; init; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; init; }

        [JsonProperty("error_message")]
        public string? ErrorMessage { get; init; }

        public int Elapsed { get; init; }

        [JsonProperty("credit_count")]
        public int CreditCount { get; init; }
    }
}
