namespace Cryptunics.Infrastructure.Client.ExchangeRates
{
    using System.Threading.Tasks;
    using Core.Domain;

    public interface IExchangeRatesClient
    {
        Task<LatestRatesResponse> GetLatestRatesAsync(FiatCoin @base, params FiatCoin[] currencies);
    }
}