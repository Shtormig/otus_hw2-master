using System.IO;

namespace otus_interfaces
{
    public interface IBudgetApplication
    {
        
        void AddTransaction(string input);
        void OutputTransactions();
        void OutputBalanceInCurrency(string currencyCode);
    }
}
