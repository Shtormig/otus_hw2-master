using System;

namespace otus_interfaces
{
    public class TransactionParser : ITransactionParser
    {
        public ITransaction Parse(string input)
        {
            var date = DateTimeOffset.Now;
            var splits = input.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var typeCode = splits[0];
            var currencyAmount = new CurrencyAmount(splits[2], decimal.Parse(splits[1]));
            switch (typeCode)
            {
                case "Трата":
                    return new Expense(currencyAmount, date, splits[3], splits[4]);
                case "Зачисление":
                    return new Income(currencyAmount, date, splits[3]);
                case "Перевод":
                    return new Transfer(currencyAmount, date, splits[3], splits[4]);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
