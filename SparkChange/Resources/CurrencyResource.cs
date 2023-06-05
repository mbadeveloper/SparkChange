using SparkChange.Contracts;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Utilities;
using System.Web;

namespace SparkChange.Resources
{
    public class CurrencyResource : ICurrencyResource
    {
        private readonly IApiClient apiClient;
        private readonly IConfiguration configuration;

        public CurrencyResource(IApiClient apiClient, IConfiguration configuration)
        {
            this.apiClient = apiClient;
            this.configuration = configuration;
        }

        public async Task<decimal> GetExchangeRate(CurrencyValue currencySource, CurrencyValue foreignCurrency)
        {
            var queryParameters = new Dictionary<string,string>
            {
                {"access_key",configuration.GetSection("CurrencyLayerApi:ApiAccessKey").Get<string>()},
                {"currencies", foreignCurrency.ToString() },
                { "source", currencySource.ToString() },
                { "format" , "1" }
            };

            var queryString = $"?{string.Join("&", queryParameters.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value)))}";

            var result = await apiClient.GetRequest<CurrencyLayerResponse>(queryString);

            var quoteKey = result?.Quotes.FirstOrDefault(q => q.Key == $"{currencySource}{foreignCurrency}").Key;

            if (quoteKey != null)
            {
                return (result?.Quotes.FirstOrDefault(q => q.Key == $"{currencySource}{foreignCurrency}").Value).Value;
            }
            else
            {
                throw new ApiClientResponseException($"Exchange rate for currency {currencySource} to {foreignCurrency} not found", StatusCodes.Status404NotFound);
            }

        }
    }
}
