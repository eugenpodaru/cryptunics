namespace Cryptunics.Core.Repository
{
    using System.Collections.Generic;
    using Domain;

    public interface ICoinRepository
    {
        Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync();

        Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync();

        Task<FiatCoin> GetFiatCoinBySymbolAsync(string symbol);

        Task<CryptoCoin> GetCryptoCoinByIdAsync(int id);
    }
}
