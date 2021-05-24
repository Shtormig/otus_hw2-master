using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace otus_interfaces
{
    public class CurrencyAmount : ICurrencyAmount, IEquatable<CurrencyAmount>
    {
        public CurrencyAmount(string currencyCode, decimal amount)
        {
            CurrencyCode = currencyCode;            
            Amount = amount;
        }

        public string CurrencyCode { get; }

        public decimal Amount { get; private set; }   
        
        public static CurrencyAmount operator +(CurrencyAmount x, ICurrencyAmount y)
        {
            if (x.CurrencyCode != y.CurrencyCode)
            {
                throw new InvalidOperationException("Currencies should be equal");
            }
            return new CurrencyAmount(x.CurrencyCode, x.Amount + y.Amount);
        }

        public static CurrencyAmount operator -(CurrencyAmount x, ICurrencyAmount y)
        {
            if (x.CurrencyCode != y.CurrencyCode)
            {
                throw new InvalidOperationException("Currencies should be equal");
            }
            return new CurrencyAmount(x.CurrencyCode, x.Amount - y.Amount);
        }

        public static bool operator ==(CurrencyAmount left, CurrencyAmount right)
        {
            return EqualityComparer<CurrencyAmount>.Default.Equals(left, right);
        }

        public static bool operator !=(CurrencyAmount left, CurrencyAmount right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{Amount:0.00} {CurrencyCode}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CurrencyAmount);
        }

        public bool Equals(CurrencyAmount other)
        {
            return other != null &&
                   CurrencyCode == other.CurrencyCode &&
                   Amount == other.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrencyCode, Amount);
        }
    }
}
