using System.ComponentModel;

namespace SparkChange.Contracts
{
    public class ProductResponse
    {
        [ReadOnly(true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductUnitValue Unit { get; set; }
        public CurrencyValue Currency { get; set; }
    }
}
