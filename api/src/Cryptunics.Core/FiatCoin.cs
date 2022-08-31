namespace Cryptunics.Core
{
    public sealed record FiatCoin(int Id, string Symbol, string Sign, string Name) : Coin(Id, Symbol, Name);
}
