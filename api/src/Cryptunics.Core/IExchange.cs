namespace Cryptunics.Core
{
    using Domain;

    public interface IExchange
    {
        Task<Quote> GetLatestQuoteAsync(int baseId);

        Task<Quote> GetLatestQuoteAsync(CryptoCoin @base);
    }
}