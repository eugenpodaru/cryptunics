namespace Cryptunics.Infrastructure.Clients.ExchangeRates
{
    using Cryptunics.Core.Domain;
    using Flurl;
    using Flurl.Http;
    using System;
    using System.Net;

    public class ExchangeRatesClient : IExchangeRatesClient
    {
        private readonly ExchangeRatesOptions _options;

        public ExchangeRatesClient(ExchangeRatesOptions options) => _options = options ?? throw new ArgumentNullException(nameof(options));

        public async Task<LatestRatesResponse> GetLatestRatesAsync(FiatCoin @base, params FiatCoin[] currencies)
        {
            try
            {
                return await _options.Url
                    .AppendPathSegment("latest")
                    .WithHeader("apikey", _options.Key)
                    .SetQueryParam("base", @base.Symbol)
                    .SetQueryParam("symbols", string.Join(",", currencies.Select(c => c.Symbol)))
                    .GetJsonAsync<LatestRatesResponse>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<ErrorResponse>();

                throw new HttpRequestException(error.Message, ex, (HttpStatusCode)ex.StatusCode.GetValueOrDefault());
            }
        }
    }
}
