using SparkChange.Contracts;

namespace SparkChange.Resources.Validators
{
    public interface IBasketValidator
    {
        void Validate(BasketItemRequest basketItemRequest);
    }
}
