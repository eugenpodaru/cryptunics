namespace Cryptunics.Core.Domain
{
    public sealed record Quote(Coin Base, params Rate[] Rates)
    {
        public static Quote Empty(Coin @base) => new(@base, Array.Empty<Rate>());
    }
}
