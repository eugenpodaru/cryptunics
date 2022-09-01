namespace Cryptunics.Core.Repository
{
    using Domain;
    using System.Collections.Generic;

    public interface IFiatCoinQuoteRepository
    {
        Quote GetLatestQuote(FiatCoin @base, IEnumerable<FiatCoin> currencies);
    }
}
