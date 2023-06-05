using System.ComponentModel.DataAnnotations;

namespace SparkChange.Resources.Validators
{
    public static class GeneralValidators
    {
        public static void ValidateCustomerId(Guid customerId)
        {
            if (!Helpers.IsGuid(customerId))
            {
                throw new ValidationException("Invalid customerId");
            }
        }
    }
}
