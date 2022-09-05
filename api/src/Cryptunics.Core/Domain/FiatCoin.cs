namespace Cryptunics.Core.Domain
{
    public sealed record FiatCoin(int Id, string Symbol, string? Sign = default, string? Name = default) : Coin(Id, Symbol, Name);
}
