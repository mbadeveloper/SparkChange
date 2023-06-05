using System.ComponentModel;

namespace SparkChange.Contracts
{
    public class BasketItemResponse
    {
        [ReadOnly(true)]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public ProductUnitValue Unit { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity; 
    }
}
