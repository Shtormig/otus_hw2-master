namespace otus_interfaces
{
    public interface ITransactionRepository
    {
        void AddTransaction(ITransaction transaction);
        ITransaction[] GetTransactions();
    }
}
