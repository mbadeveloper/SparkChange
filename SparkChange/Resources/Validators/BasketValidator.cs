using SparkChange.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SparkChange.Resources.Validators
{
    public class BasketValidator : IBasketValidator
    {
        public void Validate(BasketItemRequest basketItemRequest)
        {
            if(basketItemRequest.ProductId <= 0)
                throw new ValidationException("Invalid ProductId");

            if(basketItemRequest.Quantity <= 0)
                throw new ValidationException("Invalid Quantity");

            if (basketItemRequest.Quantity >= 100)
                throw new ValidationException("Quantity should be between 1 and 99");
        }
    }
}
