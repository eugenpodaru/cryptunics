namespace Cryptunics.Infrastructure.Clients.CoinMarketCap
{
    using Core.Domain;
    using System.Threading.Tasks;

    public interface ICoinMarketCapClient
    {
        Task<ResponseV1<CryptoCoinPayload>> GetCryptoCoinMapAsync();
        Task<ResponseV1<FiatCoinPayload>> GetFiatCoinMapAsync();
        Task<ResponseV1<ListingPayload>> GetLatestListingsAsync(FiatCoin currency);
        Task<ResponseV2<ListingPayload>> GetLatestListingsAsync(FiatCoin currency, params CryptoCoin[] listings);
    }
}