using System.Threading.Tasks;

namespace otus_interfaces
{
    public interface ICurrencyAmount
    {
        string CurrencyCode { get; }
        decimal Amount { get; }
    }
}
