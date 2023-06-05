using FluentAssertions;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Converters;
using SparkChange.Domain;
using SparkChange.Tests.Helpers;

namespace SparkChange.Tests.Converters
{
    [TestFixture]
    public class BasketConverterTests
    {
        private IBasketConverter underTest;

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new BasketConverter();
        }

        [Test]
        public void WhenBasketResponseConverterRunThenExpectFieldsMappedCorrectly()
        {
            var basket = new Basket
            {
                Id = 5,
                Currency = CurrencyValue.USD,
                CustomerId = UnitTestHelpers.CustomerId2,
                Items = new List<BasketItem>
                {
                    UnitTestHelpers.GetBasketItem7,
                    UnitTestHelpers.GetBasketItem12
                }
            };

            var basketItemResponse7 = UnitTestHelpers.GetBasketItemResponse7;
            basketItemResponse7.Price = 1.5960M;
            var basketItemResponse12 = UnitTestHelpers.GetBasketItemResponse12;
            basketItemResponse12.Price = 3.2680M;

            var expectedResult = new BasketResponse
            {
                Id = 5,
                Currency = CurrencyValue.AFN,
                CustomerId = UnitTestHelpers.CustomerId2,
                BasketItems = new List<BasketItemResponse>
                {
                    basketItemResponse7,
                    basketItemResponse12
                }
            };

            //Act
            var result = underTest.ToBasketResponse(basket, CurrencyValue.AFN, 0.76M);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void WhenBasketItemConverterRunThenExpectFieldsMappedCorrectly()
        {
            var basketItemRequest = UnitTestHelpers.GetBasketItemRequest;
            var expectedResult = UnitTestHelpers.GetBasketItem7;

            //Act
            var result = underTest.ToBasketItem(basketItemRequest);

            //Assert
            Assert.AreEqual(expectedResult.ProductId, result.ProductId);
            Assert.AreEqual(expectedResult.Quantity, result.Quantity);
        }

        [Test]
        public void WhenBasketItemResponseConverterRunThenExpectFieldsMappedCorrectly()
        {
            var basketItem = UnitTestHelpers.GetBasketItem7;
            var expectedResult = UnitTestHelpers.GetBasketItemResponse7;

            //Act
            var result = underTest.ToBasketItemResponse(basketItem, 1);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GivenDifferentExchagneRateWhenBasketItemResponseConverterRunThenExpectFieldsMappedCorrectly()
        {
            var basketItem = UnitTestHelpers.GetBasketItem7;
            var expectedResult = UnitTestHelpers.GetBasketItemResponse7;
            expectedResult.Price = 1.575M;

            //Act
            var result = underTest.ToBasketItemResponse(basketItem, 0.75M);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
