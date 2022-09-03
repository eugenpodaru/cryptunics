namespace Cryptunics.Core.Repository
{
    using Domain;

    public interface IFiatCoinQuoteRepository
    {
        Task<Quote> GetLatestQuoteAsync(FiatCoin @base, params FiatCoin[] currencies);
    }
}
