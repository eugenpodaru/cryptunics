namespace Cryptunics.Core
{
    using Domain;
    using System.Collections.Generic;

    public interface IExchange
    {
        IEnumerable<CryptoCoin> GetAllCryptoCoins();
        IEnumerable<FiatCoin> GetAllFiatCoins();
        Quote GetLatestQuote(CryptoCoin @base, params FiatCoin[] currencies);
    }
}