namespace Cryptunics.Infrastructure.Clients.ExchangeRates
{
    public record ExchangeRatesOptions
    {
        public string? Url { get; init; }

        public string? Key { get; init; }
    }
}
