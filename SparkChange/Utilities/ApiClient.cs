using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SparkChange.Resources.Validators.Exceptions;

namespace SparkChange.Utilities
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public ApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T?> GetRequest<T>(
            string requestUrl,
            IEnumerable<(string Name, string Value)>? headers = null)
            where T : class
        {
            var requestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{httpClient.BaseAddress}{requestUrl}"),
                Method = HttpMethod.Get,
            };

            AddHeaders(requestMessage, headers);

            return await SendRequest<T>(requestMessage);
        }

        private async Task<T?> SendRequest<T>(HttpRequestMessage requestMessage) where T : class
        {
            HttpResponseMessage response;

            try
            {
                response = await httpClient.SendAsync(requestMessage);
            }
            catch (Exception ex)
            {
                throw new ApiClientException($"Failed to send request to {requestMessage.RequestUri}", ex);
            }

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(res, _serializerSettings);
                }
                return null;
            }

            throw new ApiClientResponseException(
                $"Error response status returned. Url: {requestMessage.RequestUri}",
                (int)response.StatusCode,
                await response.Content.ReadAsStringAsync());
        }

        private static void AddHeaders(HttpRequestMessage requestMessage, IEnumerable<(string Name, string Value)>? headers)
        {
            foreach (var header in headers ?? Enumerable.Empty<(string Name, string Value)>())
            {
                requestMessage.Headers.Add(header.Name, header.Value);
            }
        }
    }
}
