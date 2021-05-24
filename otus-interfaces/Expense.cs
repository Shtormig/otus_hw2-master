using System;

namespace otus_interfaces
{
    public class Expense : ITransaction
    {
        public Expense(ICurrencyAmount amount, DateTimeOffset date, string category, string destination)
        {
            Amount = amount;            
            Date = date;
            Category = category;
            Destination = destination;
        }

        public ICurrencyAmount Amount { get; }
        public DateTimeOffset Date { get; }
        public string Category { get; }
        public string Destination { get; }
        public override string ToString() => $"Трата {Amount} в {Destination} по категории {Category}";
        public string ToLogStr() => $"Трата {Amount} {Category} {Destination}";
    }
}
