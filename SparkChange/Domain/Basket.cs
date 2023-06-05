using SparkChange.Contracts;

namespace SparkChange.Domain
{
    public class Basket
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public CurrencyValue Currency { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
