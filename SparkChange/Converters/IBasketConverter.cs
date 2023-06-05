using SparkChange.Contracts;
using SparkChange.Domain;

namespace SparkChange.Converters
{
    public interface IBasketConverter
    {
        BasketResponse ToBasketResponse(Basket basket, CurrencyValue selectedCurrency, decimal exchangeRate);
        BasketItemResponse ToBasketItemResponse(BasketItem basketItem, decimal exchangeRate);
        BasketItem ToBasketItem(BasketItemRequest basketItemRequest);
    }
}
