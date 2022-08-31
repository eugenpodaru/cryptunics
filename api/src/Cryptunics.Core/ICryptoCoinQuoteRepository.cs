namespace Cryptunics.Core
{
    public interface ICryptoCoinQuoteRepository
    {
        Quote GetLatestQuote(CryptoCoin @base, FiatCoin currency);
    }
}
