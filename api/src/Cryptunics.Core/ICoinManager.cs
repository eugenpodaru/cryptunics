namespace Cryptunics.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface ICoinManager
    {
        Task<IEnumerable<CryptoCoin>> GetAllCryptoCoinsAsync();

        Task<IEnumerable<FiatCoin>> GetAllFiatCoinsAsync();

        Task<CryptoCoin> GetCryptoCoinByIdAsync(int id);

        Task<FiatCoin> GetDefaultFiatCoinAsync();

        Task<IEnumerable<FiatCoin>> GetQuoteFiatCoinsAsync();
    }
}