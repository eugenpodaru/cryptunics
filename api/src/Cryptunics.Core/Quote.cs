namespace Cryptunics.Core
{
    using System.Collections.Generic;

    public sealed record Quote(Coin Base, IEnumerable<Rate> Rates);
}
