namespace Cryptunics.Core.Domain
{
    using System;

    public sealed record Rate(FiatCoin Currency, decimal Price, bool IsDerived, DateTimeOffset Timestamp);
}
