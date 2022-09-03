namespace Cryptunics.Core.Repository
{
    using Domain;
    using System.Collections.Generic;

    public interface ICoinRepository
    {
        Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync();

        Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync();

        Task<FiatCoin> GetFiatCoinByIdAsync(int id);

        Task<FiatCoin> GetFiatCoinBySymbolAsync(string symbol);

        Task<CryptoCoin> GetCryptoCoinByIdAsync(int id);
    }
}
