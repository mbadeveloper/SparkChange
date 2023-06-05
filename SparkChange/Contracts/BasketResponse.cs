using System.ComponentModel;

namespace SparkChange.Contracts
{
    public class BasketResponse
    {
        [ReadOnly(true)]
        public int Id { get; set; }

        [ReadOnly(true)]
        public Guid CustomerId { get; set; }

        public List<BasketItemResponse> BasketItems { get; set; }
        public CurrencyValue Currency { get; set; }
        public decimal Total => BasketItems?.Sum(bi => bi.Total) ?? 0;
    }
}
