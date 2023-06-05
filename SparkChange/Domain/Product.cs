using SparkChange.Contracts;

namespace SparkChange.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductUnitValue Unit { get; set; }
        public CurrencyValue Currency { get; set; }
    }
}
