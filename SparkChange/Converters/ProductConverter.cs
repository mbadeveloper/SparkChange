using SparkChange.Contracts;
using SparkChange.Domain;

namespace SparkChange.Converters
{
    public class ProductConverter : IProductConverter
    {
        public ProductResponse ToProductrResponse(Product product, CurrencyValue selectedCurrency, decimal exchangeRate)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Currency == selectedCurrency ? product.Price : (product.Price * exchangeRate),
                Unit = product.Unit,
                Currency = selectedCurrency
            };
        }
    }
}
