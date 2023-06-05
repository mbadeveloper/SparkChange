using SparkChange.Contracts;

namespace SparkChange.Resources
{
    public interface IGoodsResource
    {
        Task<IList<ProductResponse>> GetAll(CurrencyValue selectedCurrency);
    }
}
