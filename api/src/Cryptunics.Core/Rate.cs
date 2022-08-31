namespace Cryptunics.Core
{
    using System;

    public sealed record Rate(FiatCoin Currency, decimal Price, bool IsDerived, DateTimeOffset Timestamp);
}
