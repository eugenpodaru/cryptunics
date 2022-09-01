namespace Cryptunics.Core.Domain
{
    using System;

    public sealed record Rate(FiatCoin Currency, decimal Price, DateTimeOffset Timestamp, bool IsDerived = false);
}
