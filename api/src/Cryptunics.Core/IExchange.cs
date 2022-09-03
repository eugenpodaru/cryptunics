namespace Cryptunics.Core
{
    using Domain;
    using System.Collections.Generic;

    public interface IExchange
    {
        Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync();

        Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync();

        Task<Quote> GetLatestQuoteAsync(CryptoCoin @base, params FiatCoin[] currencies);
    }
}