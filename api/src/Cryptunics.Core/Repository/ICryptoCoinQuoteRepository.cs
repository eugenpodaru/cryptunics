namespace Cryptunics.Core.Repository
{
    using Domain;

    public interface ICryptoCoinQuoteRepository
    {
        Quote GetLatestQuote(CryptoCoin @base, FiatCoin currency);
    }
}
