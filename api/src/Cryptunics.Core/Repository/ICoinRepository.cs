namespace Cryptunics.Core.Repository
{
    using Domain;
    using System.Collections.Generic;

    public interface ICoinRepository
    {
        Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync();

        Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync();

        Task<FiatCoin> GetFiatCoinBySymbolAsync(string symbol);

        Task<CryptoCoin> GetCryptoCoinByIdAsync(int id);
    }
}
