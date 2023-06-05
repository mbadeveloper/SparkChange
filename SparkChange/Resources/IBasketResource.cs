using SparkChange.Contracts;

namespace SparkChange.Resources
{
    public interface IBasketResource
    {
        Task<BasketResponse> Get(Guid customerId, CurrencyValue selectedCurrency);
        Task<BasketItemResponse> Post(Guid customerId, BasketItemRequest basketItem);
        Task<bool> Delete(Guid customerId, int productId);
    }
}
