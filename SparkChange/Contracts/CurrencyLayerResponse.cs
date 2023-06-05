namespace SparkChange.Contracts
{
    public class CurrencyLayerResponse
    {
        public bool Success { get; set; }
        //public TimeSpan Timestamp { get; set; }
        public CurrencyValue Source { get; set; }
        public Dictionary<string,decimal> Quotes { get; set; }
    }
}
