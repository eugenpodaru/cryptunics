namespace Cryptunics.Infrastructure.Client.ExchangeRates
{
    using Core.Domain;
    using System.Threading.Tasks;

    public interface IExchangeRatesClient
    {
        Task<LatestRatesResponse> GetLatestRatesAsync(FiatCoin @base, params FiatCoin[] currencies);
    }
}