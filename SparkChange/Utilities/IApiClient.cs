namespace SparkChange.Utilities
{
    public interface IApiClient
    {
        Task<T?> GetRequest<T>(
            string requestUrl,
            IEnumerable<(string Name, string Value)>? headers = null)
            where T : class;
    }
}
