using SparkChange.Contracts;

namespace SparkChange.Resources
{
    public interface ICurrencyResource
    {
        Task<decimal> GetExchangeRate(CurrencyValue currencySource, CurrencyValue foreignCurrency);
    }
}
