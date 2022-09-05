namespace Cryptunics.Infrastructure.Repository
{
    using Client.CoinMarketCap;
    using Client.ExchangeRates;
    using Core.Domain;

    public static class PayloadExtensions
    {
        public static CryptoCoin ToCryptoCoin(this CryptoCoinPayload payload) => new(payload.Id, payload.Symbol ?? string.Empty, payload.Name);

        public static CryptoCoin ToCryptoCoin(this ListingPayload payload) => new(payload.Id, payload.Symbol ?? string.Empty, payload.Name);

        public static FiatCoin ToFiatCoin(this FiatCoinPayload payload) => new(payload.Id, payload.Symbol ?? string.Empty, payload.Sign, payload.Name);

        public static Quote ToQuote(this ListingPayload payload, params FiatCoin[] currencies) => new(payload.ToCryptoCoin(), payload.ToRates(currencies));

        public static Quote ToQuote(this LatestRatesResponse response, FiatCoin @base, params FiatCoin[] currencies) => new(@base, response.ToRates(currencies));

        public static Rate[] ToRates(this ListingPayload payload, params FiatCoin[] currencies)
        {
            return ToRatesEnumerable().ToArray();

            IEnumerable<Rate> ToRatesEnumerable()
            {
                foreach (var currency in currencies)
                {
                    if (!payload.Quote!.TryGetValue(currency.Id.ToString(), out var quotePayload))
                    {
                        continue;
                    }

                    var lastUpdated = string.IsNullOrEmpty(quotePayload.LastUpdated) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(quotePayload.LastUpdated);

                    yield return new Rate(currency, quotePayload.Price, lastUpdated);
                }
            }
        }

        public static Rate[] ToRates(this LatestRatesResponse response, params FiatCoin[] currencies)
        {
            var lastUpdated = DateTimeOffset.FromUnixTimeSeconds(response.Timestamp);

            return ToRatesEnumerable().ToArray();

            IEnumerable<Rate> ToRatesEnumerable()
            {
                foreach (var currency in currencies)
                {
                    if (!response.Rates!.TryGetValue(currency.Symbol, out var rate))
                    {
                        continue;
                    }

                    yield return new Rate(currency, rate, lastUpdated);
                }
            }
        }
    }
}
