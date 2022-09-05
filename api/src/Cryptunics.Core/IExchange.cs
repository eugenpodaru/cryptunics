namespace Cryptunics.Core
{
    using Domain;

    public interface IExchange
    {
        Task<Quote> GetLatestQuoteAsync(int cryptoCoinId);

        Task<Quote> GetLatestQuoteAsync(CryptoCoin cryptoCoin);
    }
}