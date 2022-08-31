namespace Cryptunics.Core
{
    public sealed record CryptoCoin(int Id, string Symbol, string Name) : Coin(Id, Symbol, Name);
}
