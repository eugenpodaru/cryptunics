namespace Cryptunics.Core.Repository
{
    using Domain;

    public interface ICryptoCoinQuoteRepository
    {
        Task<Quote> GetLatestQuoteAsync(CryptoCoin @base, FiatCoin currency);
    }
}
