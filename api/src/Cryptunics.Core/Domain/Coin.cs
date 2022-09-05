namespace Cryptunics.Core.Domain
{
    public abstract record Coin(int Id, string Symbol, string? Name = default);
}
