using System;

namespace otus_interfaces
{
    public class Transfer : ITransaction
    {
        public ICurrencyAmount Amount { get; }
        public DateTimeOffset Date { get; }

        public string Destination { get; }
        public string Message { get; }

        public override string ToString() => $"Перевод {Amount} на имя {Destination} с сообщением {Message}";
        public  string ToLogStr() => $"Перевод {Amount} {Destination} {Message}";
        //{
        //    return($"Перевод {Amount} на имя {Destination} с сообщением {Message}");
        //}
        

        public Transfer(ICurrencyAmount amount, DateTimeOffset date, string destination, string message)
        {
            Amount = amount;
            Date = date;
            Destination = destination;
            Message = message;
        }


    }
}
