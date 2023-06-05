using Microsoft.EntityFrameworkCore;
using SparkChange.Contracts;
using SparkChange.Converters;
using SparkChange.Domain;
using SparkChange.Resources.Validators;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SparkChange.Resources
{
    public class BasketResource : IBasketResource
    {
        private readonly DatabaseContext databaseContext;        
        private readonly ICurrencyResource currencyResource;
        private readonly IBasketConverter basketConverter;
        private readonly IBasketValidator basketValidator;

        public BasketResource(DatabaseContext databaseContext,
            ICurrencyResource currencyResource,
            IBasketConverter basketConverter,            
            IBasketValidator basketValidator)
        {
            this.databaseContext = databaseContext;            
            this.currencyResource = currencyResource;
            this.basketConverter = basketConverter;
            this.basketValidator = basketValidator;
        }

        public async Task<BasketResponse> Get(Guid customerId, CurrencyValue selectedCurrency)
        {
            GeneralValidators.ValidateCustomerId(customerId);

            decimal exchangeRate = 1;
            var basket = await GetCustomerBasket(customerId);

            if (basket.Currency != selectedCurrency && basket?.Items != null && basket.Items.Any())
            {
                exchangeRate = await currencyResource.GetExchangeRate(basket.Currency, selectedCurrency);
            }

            return basketConverter.ToBasketResponse(basket, selectedCurrency, exchangeRate);
        }

        public async Task<BasketItemResponse> Post(Guid customerId, BasketItemRequest basketItemRequest)
        {
            GeneralValidators.ValidateCustomerId(customerId);
            basketValidator.Validate(basketItemRequest);

            await ProductExists(basketItemRequest.ProductId);

            decimal exchangeRate = 1;
            var basket = await GetCustomerBasket(customerId);
            var customerBasketItem = basket.Items?.FirstOrDefault(i => i.ProductId == basketItemRequest.ProductId);

            if (customerBasketItem == null)
            {
                if (basket.Items == null)
                {
                    basket.Items = new List<BasketItem>();
                }

                var basketItem = basketConverter.ToBasketItem(basketItemRequest);
                basket.Items.Add(basketItem);
            }
            else
            {
                customerBasketItem.Quantity += basketItemRequest.Quantity;
            }

            await databaseContext.SaveChangesAsync();

            var newBasketItem = await GetBasketItem(basket.Id, basketItemRequest.ProductId);

            return basketConverter.ToBasketItemResponse(newBasketItem, exchangeRate);
        }

        public async Task<bool> Delete(Guid customerId, int productId)
        {
            GeneralValidators.ValidateCustomerId(customerId);

            if (productId <= 0)
            {
                throw new ValidationException("Invalid productId");
            }

            await ProductExists(productId);

            var basket = await GetCustomerBasket(customerId);
            var customerBasketItem = basket.Items?.FirstOrDefault(i => i.ProductId == productId);

            if (customerBasketItem != null)
            {
                basket.Items.Remove(customerBasketItem);
                await databaseContext.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ApiClientResponseException("Product not exists in customer basket", StatusCodes.Status404NotFound);
            }
        }

        private async Task<Basket> GetCustomerBasket(Guid customerId)
        {
            var basket = await databaseContext
                .Baskets
                .Include(b => b.Items)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.CustomerId == customerId);

            if (basket == null)
            {
                basket = new Basket
                {
                    CustomerId = customerId,
                    Currency = ApplicationConstants.DefaultCurrency
                };

                databaseContext.Baskets.Add(basket);
                databaseContext.SaveChanges();
            }

            return basket;
        }

        private async Task<BasketItem> GetBasketItem(int basketId, int productId)
        {
            return await databaseContext
                .BasketItems
                .AsNoTracking()
                .Include(bi => bi.Product)
                .FirstOrDefaultAsync(bi => bi.BasketId == basketId && bi.ProductId == productId);
        }

        private async Task ProductExists(int productId)
        {
            var product = await databaseContext
                    .Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new ValidationException("Product not exist in the list of products");
            }
        }
    }
}
