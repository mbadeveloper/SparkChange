using SparkChange.Contracts;
using SparkChange.Domain;

namespace SparkChange.Converters
{
    public interface IProductConverter
    {
        ProductResponse ToProductrResponse(Product product, CurrencyValue selectedCurrency, decimal exchangeRate);
    }
}
