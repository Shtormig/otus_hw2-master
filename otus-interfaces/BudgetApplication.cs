using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace otus_interfaces
{
    public class BudgetApplication : IBudgetApplication
    {
        private readonly ICurrencyConverter _currencyConverter;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionParser _transactionParser;
        private readonly string _basecurrency;
        private readonly List<string> _acceptedcurrencylist;

        public BudgetApplication(ITransactionRepository transactionRepository, ITransactionParser transactionParser, ICurrencyConverter currencyConverter, string basecurrency,  List<string>currlst)
        {
            _currencyConverter = currencyConverter;
            _transactionRepository = transactionRepository;
            _transactionParser = transactionParser;
            _basecurrency = basecurrency;
            _acceptedcurrencylist = currlst;
        }

        public void AddTransaction(string input)
        {
            var transaction = _transactionParser.Parse(input);

            if (!_acceptedcurrencylist.Contains(transaction.Amount.CurrencyCode))
            {
                Console.WriteLine($"Транзакции в {transaction.Amount.CurrencyCode} не поддерживаются");
                return;
            }
            _transactionRepository.AddTransaction(transaction);
        }

        public void OutputTransactions()
        {
            foreach (var transaction in _transactionRepository.GetTransactions())
            {
                Console.WriteLine(transaction);
            }
        }

        public void OutputBalanceInCurrency(string currencyCode)
        {
            var totalCurrencyAmount = new CurrencyAmount(_basecurrency, 0);
            var amounts = _transactionRepository.GetTransactions()
                .Select(t => t.Amount)
                .Select(a => a.CurrencyCode != _basecurrency ? _currencyConverter.ConvertCurrency(a, _basecurrency) : a)
                .ToArray();

            var totalBalanceAmount = _currencyConverter.ConvertCurrency(amounts.Aggregate(totalCurrencyAmount, (b, a) => b += a), currencyCode);

            
            Console.WriteLine($"Остаток на счете: {totalBalanceAmount}");
        }
        public void OutputBalanceInMainCurrencies()
        {
            Console.WriteLine("======================");
            foreach (string curn in _acceptedcurrencylist)
                {
                OutputBalanceInCurrency(curn);
            }
            //OutputBalanceInCurrency("EUR");
            //OutputBalanceInCurrency("USD");
            //OutputBalanceInCurrency("RUB");
            //OutputBalanceInCurrency("KZT");

            Console.WriteLine("======================");

        }

    }
}
