namespace Cryptunics.Core.Domain
{
    public sealed record CryptoCoin(int Id, string Symbol, string? Name = default) : Coin(Id, Symbol, Name);
}
