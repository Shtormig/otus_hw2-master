using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace otus_interfaces
{
   
    public class TransactionRepository : ITransactionRepository
    {
        private readonly List<ITransaction> _transactions = new List<ITransaction>();
        private readonly string _path;
        private readonly ITransactionParser _parser;

        public TransactionRepository(string path, ITransactionParser parser)
        {
            _path = path;
            _parser = parser;

            LoadFromFile();
        }

        public void AddTransaction(ITransaction transaction)
        {
            _transactions.Add(transaction);

            using (StreamWriter file = new StreamWriter(_path, append: true))
            {
                file.WriteLine(transaction.ToLogStr());
            }

        }

        public ITransaction[] GetTransactions()
        {
            return _transactions.ToArray();
        }

        private void LoadFromFile()
        {
            _transactions.Clear();

            if (!File.Exists(_path))
            {
                var fs = File.Create(_path);
                fs.Close();
            }

            var lines = File.ReadAllLines(_path);

            foreach (var line in lines)
            {
                try
                {
                    var transaction = _parser.Parse(line);
                    _transactions.Add(transaction);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Ошибка при разборе строки: {line}. Сообщение: {e.Message}");
                }
            }
        }
    }
}
