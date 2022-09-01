﻿namespace Cryptunics.Core.Repository
{
    using Domain;
    using System.Collections.Generic;

    public interface ICoinRepository
    {
        IEnumerable<FiatCoin> GetAllFiatCoins();
        IEnumerable<CryptoCoin> GetAllCryptoCoins();
        FiatCoin GetFiatCoinById(int id);
        FiatCoin GetFiatCoinBySymbol(string symbol);
        CryptoCoin GetCryptoCoinById(int id);
    }
}