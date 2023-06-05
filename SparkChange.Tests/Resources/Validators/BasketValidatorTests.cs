using FluentAssertions;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Resources.Validators;
using System.ComponentModel.DataAnnotations;

namespace SparkChange.Tests.Resources.Validators
{
    [TestFixture]
    public class BasketValidatorTests
    {
        private IBasketValidator underTest;

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new BasketValidator();
        }

        private static IEnumerable<TestCaseData> InvalidProductId() => new[]
        {
            new TestCaseData(0),
            new TestCaseData(-1)
        };

        private static IEnumerable<TestCaseData> InvalidQuantity() => new[]
        {
            new TestCaseData(0),
            new TestCaseData(-1)
        };

        [Test, TestCaseSource(nameof(InvalidProductId))]
        public void GivenBasketItemRequestWithInvalidProductIdThrowException(int productId)
        {
            var basketItemRequest = new BasketItemRequest
            {
                ProductId = productId,
                Quantity = 4
            };

            //Act
            void Action() => underTest.Validate(basketItemRequest);

            //Assert           
            var exception = Assert.Throws<ValidationException>(Action);
            exception.Message.Should().Be("Invalid ProductId");
        }

        [Test, TestCaseSource(nameof(InvalidQuantity))]
        public void GivenBasketItemRequestWithInvalidQuantityThrowException(int quantity)
        {
            var basketItemRequest = new BasketItemRequest
            {
                ProductId = 21,
                Quantity = quantity
            };

            //Act
            void Action() => underTest.Validate(basketItemRequest);

            //Assert           
            var exception = Assert.Throws<ValidationException>(Action);
            exception.Message.Should().Be("Invalid Quantity");
        }

        [Test]
        public void GivenBasketItemRequestWithInvalidQuantityThrowException()
        {
            var random = new Random();
            var quantity = random.Next(100, int.MaxValue);
            var basketItemRequest = new BasketItemRequest
            {
                ProductId = 21,
                Quantity = quantity
            };

            //Act
            void Action() => underTest.Validate(basketItemRequest);

            //Assert           
            var exception = Assert.Throws<ValidationException>(Action);
            exception.Message.Should().Be("Quantity should be between 1 and 99");
        }
    }
}
