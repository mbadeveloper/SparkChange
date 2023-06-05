using SparkChange.Contracts;
using SparkChange.Domain;

namespace SparkChange.Converters
{
    public class BasketConverter : IBasketConverter
    {
        public BasketConverter()
        {

        }

        public BasketResponse ToBasketResponse(Basket basket, CurrencyValue selectedCurrency, decimal exchangeRate)
        {
            return new BasketResponse
            {
                Id = basket.Id,
                CustomerId = basket.CustomerId,
                Currency = selectedCurrency,
                BasketItems = basket.Items?.Select(bi => ToBasketItemResponse(bi, exchangeRate)).ToList()
            };
        }

        public BasketItem ToBasketItem(BasketItemRequest basketItemRequest)
        {
            return new BasketItem
            {
                ProductId = basketItemRequest.ProductId,
                Quantity = basketItemRequest.Quantity
            };
        }

        public BasketItemResponse ToBasketItemResponse(BasketItem basketItem, decimal exchangeRate)
        {
            return new BasketItemResponse
            {
                Id = basketItem.Id,
                ProductName = basketItem.Product.Name,
                Unit = basketItem.Product.Unit,
                Price = exchangeRate == 1 ? basketItem.Product.Price : basketItem.Product.Price * exchangeRate,
                Quantity = basketItem.Quantity
            };
        }
    }
}
