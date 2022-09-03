namespace Cryptunics.Infrastructure.Client.CoinMarketCap
{
    using Cryptunics.Core.Domain;
    using Flurl;
    using Flurl.Http;
    using System;
    using System.Net;

    public class CoinMarketCapClient : ICoinMarketCapClient
    {
        private readonly CoinMarketCapOptions _options;

        public CoinMarketCapClient(CoinMarketCapOptions options) => _options = options ?? throw new ArgumentNullException(nameof(options));

        public Task<ResponseV1<FiatCoinPayload>> GetFiatCoinMapAsync() => TryCatch(
            () => _options.Url
                    .AppendPathSegments("v1", "fiat", "map")
                    .WithHeaders(GetHeaders())
                    .GetJsonAsync<ResponseV1<FiatCoinPayload>>());

        public Task<ResponseV1<CryptoCoinPayload>> GetCryptoCoinMapAsync() => TryCatch(
            () => _options.Url
                    .AppendPathSegments("v1", "cryptocurrency", "map")
                    .WithHeaders(GetHeaders())
                    .GetJsonAsync<ResponseV1<CryptoCoinPayload>>());

        public Task<ResponseV1<ListingPayload>> GetLatestListingsAsync(FiatCoin currency) => TryCatch(
            () => _options.Url
                    .AppendPathSegments("v1", "cryptocurrency", "listings", "latest")
                    .WithHeaders(GetHeaders())
                    .SetQueryParam("convert_id", currency.Id)
                    .GetJsonAsync<ResponseV1<ListingPayload>>());

        public Task<ResponseV2<ListingPayload>> GetLatestListingsAsync(FiatCoin currency, params CryptoCoin[] listings) => TryCatch(
            () => _options.Url
                    .AppendPathSegments("v1", "cryptocurrency", "quotes", "latest")
                    .WithHeaders(GetHeaders())
                    .SetQueryParam("id", string.Join(",", listings.Select(l => l.Id)))
                    .SetQueryParam("convert_id", currency.Id)
                    .GetJsonAsync<ResponseV2<ListingPayload>>());

        private Dictionary<string, string?> GetHeaders() => new()
        {
            { "X-CMC_PRO_API_KEY", _options.Key },
            { "Accept", "application/json" },
            { "Accept-Encoding", "deflate, gzip" }
        };

        private static async Task<T> TryCatch<T>(Func<Task<T>> httpCall)
        {
            try
            {
                return await httpCall();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<ErrorResponse>();

                throw new HttpRequestException(error.Status?.ErrorMessage, ex, (HttpStatusCode)ex.StatusCode.GetValueOrDefault());
            }
        }
    }
}
