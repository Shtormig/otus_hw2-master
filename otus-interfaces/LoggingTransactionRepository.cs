using System.Diagnostics;

namespace otus_interfaces
{
    public class LoggingTransactionRepository : ITransactionRepository // декоратор
    {
        private ITransactionRepository transactionRepository;

        public LoggingTransactionRepository(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        public void AddTransaction(ITransaction transaction)
        {
            Trace.WriteLine("AddTransaction");
            transactionRepository.AddTransaction(transaction);
        }

        public ITransaction[] GetTransactions()
        {
            Trace.WriteLine("GetTransactions");
            return transactionRepository.GetTransactions();
        }

        


    }
}
