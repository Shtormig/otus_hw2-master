using System;

namespace otus_interfaces
{
    public interface ITransaction
    {
        DateTimeOffset Date { get; }
        ICurrencyAmount Amount { get; }
        string ToLogStr();
     }
}
