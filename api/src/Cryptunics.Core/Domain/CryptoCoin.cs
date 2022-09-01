namespace Cryptunics.Core.Domain
{
    public sealed record CryptoCoin(int Id, string Symbol, string Name) : Coin(Id, Symbol, Name);
}
