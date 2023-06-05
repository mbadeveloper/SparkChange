using SparkChange.Contracts;
using SparkChange.Domain;

namespace SparkChange.Tests.Helpers
{
    public static class UnitTestHelpers
    {
        public static Guid CustomerId1 => new Guid("1c3ad083-03bc-4061-9a2a-74c9e7749e3f");
        public static Guid CustomerId2 => new Guid("746b5df2-4bb7-400e-b234-ce68be915fa6");
        public static Guid CustomerId3 => new Guid("6ef26319-5dcb-41eb-b0be-a9b93b9f31b0");

        public static Product GetProduct10 => new Product
        {
            Id = 10,
            Name = "Product10",
            Currency = CurrencyValue.USD,
            Price = 1M,
            Unit = ProductUnitValue.Loaf
        };

        public static Product GetProduct21 => new Product
        {
            Id = 21,
            Name = "Product21",
            Currency = CurrencyValue.USD,
            Price = 2.10M,
            Unit = ProductUnitValue.Bottle
        };

        public static Product GetProduct32 => new Product
        {
            Id = 32,
            Name = "Product32",
            Currency = CurrencyValue.USD,
            Price = 3.20M,
            Unit = ProductUnitValue.Tin
        };

        public static Product GetProduct43 => new Product
        {
            Id = 43,
            Name = "Product43",
            Currency = CurrencyValue.USD,
            Price = 4.30M,
            Unit = ProductUnitValue.Bag
        };

        public static Product GetProduct55 => new Product
        {
            Id = 55,
            Name = "Product55",
            Currency = CurrencyValue.USD,
            Price = 5.55M,
            Unit = ProductUnitValue.Bottle
        };

        public static IEnumerable<Product> GetProducts() => new List<Product>()
            {
               GetProduct10,
               GetProduct21,
               GetProduct32,
               GetProduct43,
               GetProduct55
            };

        public static IEnumerable<Basket> GetBaskets() => new List<Basket>
        {
            new Basket
            {
                Id = 100,
                Currency = CurrencyValue.USD,
                CustomerId = CustomerId1
            },
            new Basket
            {
                Id = 105,
                Currency = CurrencyValue.USD,
                CustomerId = CustomerId2
            },
            new Basket
            {
                Id = 107,
                Currency = CurrencyValue.USD,
                CustomerId = CustomerId3
            }
        };

        public static BasketItem GetBasketItem7 => new BasketItem
        {
            Id = 7,
            BasketId = 100,
            ProductId = 21,
            Product = GetProduct21,
            Quantity = 4
        };

        public static BasketItemResponse GetBasketItemResponse7 => new BasketItemResponse
        {
            Id = 7,
            ProductName = GetProduct21.Name,
            Unit = GetProduct21.Unit,
            Price = GetProduct21.Price,
            Quantity = 4
        };

        public static BasketItem GetBasketItem12 => new BasketItem
        {
            Id = 12,
            BasketId = 100,
            ProductId = 43,
            Product = GetProduct43,
            Quantity = 5
        };

        public static BasketItemResponse GetBasketItemResponse12 => new BasketItemResponse
        {
            Id = 12,
            ProductName = GetProduct43.Name,
            Unit = GetProduct43.Unit,
            Price = GetProduct43.Price,
            Quantity = 5
        };

        public static IEnumerable<BasketItem> GetBasketsItems() => new List<BasketItem>
        {
            new BasketItem
            {
                Id = 1,
                BasketId = 100,
                ProductId = 21,
                Quantity = 4
            },
            new BasketItem
            {
                Id = 3,
                BasketId = 100,
                ProductId = 43,
                Quantity = 1
            },
            new BasketItem
            {
                Id = 21,
                BasketId = 105,
                ProductId = 32,
                Quantity = 6
            },
            new BasketItem
            {
                Id = 23,
                BasketId = 100,
                ProductId = 55,
                Quantity = 3
            },
            new BasketItem
            {
                Id = 33,
                BasketId = 107,
                ProductId = 10,
                Quantity = 1
            },
            new BasketItem
            {
                Id = 35,
                BasketId = 107,
                ProductId = 21,
                Quantity = 5
            }
        };

        public static ProductResponse GetProductResponse10(CurrencyValue currencyValue, decimal exchangeRate) => new ProductResponse
        {
            Id = 10,
            Name = "Product10",
            Currency = currencyValue,
            Price = 1M * exchangeRate,
            Unit = ProductUnitValue.Loaf
        };

        public static ProductResponse GetProductResponse21(CurrencyValue currencyValue, decimal exchangeRate) => new ProductResponse
        {
            Id = 21,
            Name = "Product21",
            Currency = currencyValue,
            Price = 2.10M * exchangeRate,
            Unit = ProductUnitValue.Bottle
        };

        public static ProductResponse GetProductResponse32(CurrencyValue currencyValue, decimal exchangeRate) => new ProductResponse
        {
            Id = 32,
            Name = "Product32",
            Currency = currencyValue,
            Price = 3.20M * exchangeRate,
            Unit = ProductUnitValue.Tin
        };

        public static ProductResponse GetProductResponse43(CurrencyValue currencyValue, decimal exchangeRate) => new ProductResponse
        {
            Id = 43,
            Name = "Product43",
            Currency = currencyValue,
            Price = 4.30M * exchangeRate,
            Unit = ProductUnitValue.Bag
        };

        public static ProductResponse GetProductResponse55(CurrencyValue currencyValue, decimal exchangeRate) => new ProductResponse
        {
            Id = 55,
            Name = "Product55",
            Currency = currencyValue,
            Price = 5.55M * exchangeRate,
            Unit = ProductUnitValue.Bottle
        };
        public static IEnumerable<ProductResponse> GetProductsResponse(CurrencyValue currencyValue, decimal exchangeRate) => new List<ProductResponse>()
            {
               GetProductResponse10(currencyValue, exchangeRate),
               GetProductResponse21(currencyValue, exchangeRate),
               GetProductResponse32(currencyValue, exchangeRate),
               GetProductResponse43(currencyValue, exchangeRate),
               GetProductResponse55(currencyValue, exchangeRate)
            };

        public static BasketResponse GetBasketResponseCustomer1()
        {
            return new BasketResponse
            {
                Id = 100,
                Currency = CurrencyValue.USD,
                CustomerId = CustomerId1,
                BasketItems = new List<BasketItemResponse>
                {
                    new BasketItemResponse
                    {
                        Id = 1,
                        Price = GetProduct21.Price,
                        ProductName = GetProduct21.Name,
                        Quantity = 4,
                        Unit = GetProduct21.Unit
                    },
                    new BasketItemResponse
                    {
                        Id = 3,
                        Price = GetProduct43.Price,
                        ProductName = GetProduct43.Name,
                        Quantity = 1,
                        Unit = GetProduct43.Unit
                     },
                    new BasketItemResponse
                    {
                        Id = 23,
                        Price = GetProduct55.Price,
                        ProductName = GetProduct55.Name,
                        Quantity = 3,
                        Unit = GetProduct55.Unit
                     }
                }
            };
        }

        public static BasketResponse GetBasketResponseCustomer2()
        {
            return new BasketResponse
            {
                Id = 105,
                Currency = CurrencyValue.USD,
                CustomerId = CustomerId2,
                BasketItems = new List<BasketItemResponse>
                {
                    new BasketItemResponse
                    {
                        Id = 21,
                        Price = GetProduct32.Price,
                        ProductName = GetProduct32.Name,
                        Quantity = 6,
                        Unit = GetProduct32.Unit
                    }
                }
            };
        }

        public static BasketResponse GetBasketResponseCustomer3()
        {
            return new BasketResponse
            {
                Id = 107,
                Currency = CurrencyValue.USD,
                CustomerId = CustomerId3,
                BasketItems = new List<BasketItemResponse>
                {
                    new BasketItemResponse
                    {
                        Id = 33,
                        Price = GetProduct10.Price,
                        ProductName = GetProduct10.Name,
                        Quantity = 1,
                        Unit = GetProduct10.Unit
                    },
                    new BasketItemResponse
                    {
                        Id = 35,
                        Price = GetProduct21.Price,
                        ProductName = GetProduct10.Name,
                        Quantity = 5,
                        Unit = GetProduct10.Unit
                    }
                }
            };
        }

        public static BasketItemRequest GetBasketItemRequest => new BasketItemRequest
        {
            ProductId = 21,
            Quantity = 4
        };
    }
}
