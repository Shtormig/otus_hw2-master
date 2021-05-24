using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace otus_interfaces
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var currencyConverter = new ExchangeRatesApiConverter(new HttpClient(), new MemoryCache(new MemoryCacheOptions()), "a5cf9da55cb835d0a633a7825b3aa8b5", "RUB");

            //var transactionRepository = new InMemoryTransactionRepository();
            
            var transactionParser = new TransactionParser();
            var transactionRepository = new TransactionRepository("Transactionlog.txt", transactionParser);

            List<string> currlst = new List<string> { "USD", "RUB", "EUR", "KZT" };

            var budgetApp = new BudgetApplication(transactionRepository, transactionParser, currencyConverter, "RUB", currlst);

            //budgetApp.AddTransaction("Зачисление 15000 RUB пополнение_баланса");
            //budgetApp.AddTransaction("Трата -400 RUB Продукты Пятерочка");
            //budgetApp.AddTransaction("Трата -2000 RUB Бензин IRBIS");
            //budgetApp.AddTransaction("Трата -500 RUB Кафе Шоколадница");
            //budgetApp.AddTransaction("Перевод -1000 RUB Михаил За_домашку");
            //budgetApp.OutputBalanceInCurrency("RUB");
            //budgetApp.OutputBalanceInCurrency("USD");
            //budgetApp.OutputBalanceInCurrency("EUR");
            budgetApp.OutputTransactions();
            budgetApp.OutputBalanceInMainCurrencies();

            while (true)
            {
                Console.WriteLine("Введите add - для ввода новой транзакции, list - вывести транзакции, balance - остаток на счете, exit - Выход");
                var key = Console.ReadLine();
                Console.WriteLine();

                switch (key)
                {
                    case "exit":
                        return;

                    case "add":
                        Console.WriteLine("Введите транзакцию: ");
                        var input = Console.ReadLine();
                        budgetApp.AddTransaction(input);
                        break;
                    case "list":
                        Console.WriteLine("Список транзакций:");
                        Console.WriteLine("======================");
                        budgetApp.OutputTransactions();
                        break;
                    case "balance":
                        budgetApp.OutputBalanceInMainCurrencies();
                        break;
                    default:
                        Console.WriteLine("Неверный формат команды");
                        break;
                }
            }

           //Console.Read();
        }
    }
}
