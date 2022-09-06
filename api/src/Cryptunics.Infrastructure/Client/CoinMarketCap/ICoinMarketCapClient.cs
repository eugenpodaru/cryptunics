namespace Cryptunics.Infrastructure.Client.CoinMarketCap
{
    using System.Threading.Tasks;
    using Core.Domain;

    public interface ICoinMarketCapClient
    {
        Task<ResponseV1<CryptoCoinPayload>> GetCryptoCoinMapAsync();
        Task<ResponseV1<FiatCoinPayload>> GetFiatCoinMapAsync();
        Task<ResponseV1<ListingPayload>> GetLatestListingsAsync(FiatCoin currency);
        Task<ResponseV2<ListingPayload>> GetLatestListingsAsync(FiatCoin currency, params CryptoCoin[] listings);
    }
}