using System;

namespace otus_interfaces
{
    public class Comission : ITransaction // декоратор
    {
        public ITransaction OriginalTransaction { get; }
        public ICurrencyAmount Amount { get; }
        public DateTimeOffset Date { get; }
        public string ToLogStr() => $"Комиссия в размере {Amount} {OriginalTransaction}";
        public override string ToString() => $"Комиссия в размере {Amount} за транзакцию: {OriginalTransaction}";
    }
}
