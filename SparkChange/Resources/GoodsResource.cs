using Microsoft.EntityFrameworkCore;
using SparkChange.Contracts;
using SparkChange.Converters;
using SparkChange.Domain;
using SparkChange.Utilities;

namespace SparkChange.Resources
{
    public class GoodsResource : IGoodsResource
    {
        private readonly DatabaseContext databaseContext;
        private readonly ICurrencyResource currencyResource;
        private readonly IProductConverter productConverter;
        
        public GoodsResource(DatabaseContext databaseContext,
            ICurrencyResource currencyResource,
            IProductConverter productConverter)
        {
            this.databaseContext = databaseContext;
            this.currencyResource = currencyResource;
            this.productConverter = productConverter;            
        }

        public async Task<IList<ProductResponse>> GetAll(CurrencyValue selectedCurrency)
        {
            decimal exchangeRate = 1;
            var products = await databaseContext
                                    .Products
                                    .AsNoTracking()
                                    .ToListAsync();
            
            if (ApplicationConstants.DefaultCurrency != selectedCurrency)
            {
                exchangeRate = await currencyResource.GetExchangeRate(ApplicationConstants.DefaultCurrency, selectedCurrency);
            }

            return products.Select(p => productConverter.ToProductrResponse(p, selectedCurrency, exchangeRate)).ToList();
        }
    }
}
