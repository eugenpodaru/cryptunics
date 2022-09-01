namespace Cryptunics.Core.Repository
{
    using Domain;

    public interface IFiatCoinQuoteRepository
    {
        Quote GetLatestQuote(FiatCoin @base, params FiatCoin[] currencies);
    }
}
