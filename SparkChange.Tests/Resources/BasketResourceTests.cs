using FluentAssertions;
using Moq;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Converters;
using SparkChange.Resources;
using SparkChange.Resources.Validators;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Tests.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SparkChange.Tests.Resources
{
    [TestFixture]
    public class BasketResourceTests : AbstractDbTests
    {
        private IBasketResource underTest;
        private readonly Mock<ICurrencyResource> mockCurrencyResource = new Mock<ICurrencyResource>();
        private readonly IBasketConverter basketConverter = new BasketConverter();
        private readonly Mock<IBasketValidator> mockBasketValidator = new Mock<IBasketValidator>();

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new BasketResource(databaseContext, mockCurrencyResource.Object, basketConverter, mockBasketValidator.Object);
        }

        [TearDown]
        public void TearDown()
        {
            mockBasketValidator.Invocations.Clear();
        }

        private static IEnumerable<TestCaseData> InvalidCustomerId() => new[]
        {
            new TestCaseData(new Guid("00000000-0000-0000-0000-000000000000"))
        };

        private static IEnumerable<TestCaseData> InvalidProductId() => new[]
{
            new TestCaseData(0),
            new TestCaseData(-1)
        };

        private static IEnumerable<TestCaseData> ClientBasketCases() => new[]
        {
            new TestCaseData(UnitTestHelpers.CustomerId1, UnitTestHelpers.GetBasketResponseCustomer1()),
            new TestCaseData(UnitTestHelpers.CustomerId2, UnitTestHelpers.GetBasketResponseCustomer2()),
            new TestCaseData(UnitTestHelpers.CustomerId3, UnitTestHelpers.GetBasketResponseCustomer3())
        };

        [Test, TestCaseSource(nameof(InvalidCustomerId))]
        public void GivenInvalidCustomerIdParameterWhenGetShouldThrowException(Guid invalidCustomerId)
        {
            //Act
            AsyncTestDelegate action = async () => { await underTest.Get(invalidCustomerId, CurrencyValue.EUR); };

            //Assert           
            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Invalid customerId");
        }

        [Test, TestCaseSource(nameof(ClientBasketCases))]

        public async Task GivenExistingCustomerWhenGetShouldReturnValidBasket(Guid customerId, BasketResponse customerBasket)
        {
            //Act
            var result = await underTest.Get(customerId, CurrencyValue.USD);

            //Assert
            Assert.IsNotNull(result);
            result.Should().BeEquivalentTo(customerBasket);
        }

        [Test]
        public async Task GivenNewCustomerWhenGetShouldReturnEmptyBasket()
        {
            var customerId = new Guid("3a32e716-92ce-4940-acb7-a0f32816aa13");
            var customerBasket = new BasketResponse
            {
                CustomerId = customerId,
                Currency = CurrencyValue.USD
            };

            //Act
            var result = await underTest.Get(customerId, CurrencyValue.USD);

            //Assert
            Assert.IsNotNull(result);
            Assert.That(customerBasket.CustomerId, Is.EqualTo(result.CustomerId));
            Assert.That(customerBasket.Currency, Is.EqualTo(result.Currency));
            Assert.IsNull(customerBasket.BasketItems);
        }

        [Test, TestCaseSource(nameof(InvalidCustomerId))]
        public void GivenInvalidCustomerIdParameterWhenPostShouldThrowException(Guid invalidCustomerId)
        {
            var basketItemRequest = new BasketItemRequest
            {
                ProductId = 21,
                Quantity = 4
            };

            //Act
            AsyncTestDelegate action = async () => { await underTest.Post(invalidCustomerId, basketItemRequest); };

            //Assert           
            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Invalid customerId");
        }

        [Test]
        public void GivenNotExistProductWhenPostShouldThrowException()
        {
            var basketItemRequest = new BasketItemRequest
            {
                ProductId = 999,
                Quantity = 4
            };

            //Act
            AsyncTestDelegate action = async () => { await underTest.Post(UnitTestHelpers.CustomerId2, basketItemRequest); };

            //Assert           
            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Product not exist in the list of products");
        }

        [Test]
        public async Task GivenAddProductWhenPostShouldAddProductToCustomerBasket()
        {
            var customerId = UnitTestHelpers.CustomerId3;
            var basketItemRequest = new BasketItemRequest
            {
                ProductId = UnitTestHelpers.GetProduct43.Id,
                Quantity = 2
            };

            var expectedBasketItemResponse = new BasketItemResponse
            {
                ProductName = UnitTestHelpers.GetProduct43.Name,
                Price = UnitTestHelpers.GetProduct43.Price,
                Unit = UnitTestHelpers.GetProduct43.Unit,
                Quantity = 2
            };

            //Act
            var result = await underTest.Post(customerId, basketItemRequest);

            //Assert
            mockBasketValidator.Verify(v => v.Validate(It.IsAny<BasketItemRequest>()), Times.Once);
            result.Should().BeEquivalentTo(expectedBasketItemResponse, options => options.Excluding(su => su.Id));
        }

        [Test]
        public async Task GivenAddExistingItemWhenPostShouldIncreaseQuantityInCustomerBasket()
        {
            var customerId = UnitTestHelpers.CustomerId3;
            var basketItemRequest = new BasketItemRequest
            {
                ProductId = UnitTestHelpers.GetProduct10.Id,
                Quantity = 4
            };

            var expectedBasketItemResponse = new BasketItemResponse
            {
                Id = 33,
                ProductName = UnitTestHelpers.GetProduct10.Name,
                Price = UnitTestHelpers.GetProduct10.Price,
                Unit = UnitTestHelpers.GetProduct10.Unit,
                Quantity = 5
            };

            //Act
            var result = await underTest.Post(customerId, basketItemRequest);

            //Assert
            mockBasketValidator.Verify(v => v.Validate(It.IsAny<BasketItemRequest>()), Times.Once);
            result.Should().BeEquivalentTo(expectedBasketItemResponse);
        }

        [Test, TestCaseSource(nameof(InvalidCustomerId))]
        public void GivenInvalidCustomerIdParameterWhenDeleteShouldThrowException(Guid invalidCustomerId)
        {

            //Act
            AsyncTestDelegate action = async () => { await underTest.Delete(invalidCustomerId, 21); };

            //Assert           
            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Invalid customerId");
        }

        [Test, TestCaseSource(nameof(InvalidProductId))]
        public void GivenInvalidProductIdParameterWhenDeleteShouldThrowException(int invalidProductId)
        {

            //Act
            AsyncTestDelegate action = async () => { await underTest.Delete(UnitTestHelpers.CustomerId1, invalidProductId); };

            //Assert           
            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Invalid productId");
        }

        [Test]
        public void GivenNotExistProductWhenDeleteShouldThrowException()
        {
            //Act
            AsyncTestDelegate action = async () => { await underTest.Delete(UnitTestHelpers.CustomerId2, 1000); };

            //Assert           
            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Product not exist in the list of products");
        }

        [Test]
        public void GivenNotExistProductInCustomerBasketWhenDeleteShouldThrowException()
        {
            //Act
            AsyncTestDelegate action = async () => { await underTest.Delete(UnitTestHelpers.CustomerId2, 55); };

            //Assert           
            var exception = Assert.ThrowsAsync<ApiClientResponseException>(action);
            exception.Message.Should().Be("Product not exists in customer basket");
        }

        [Test]
        public async Task GivenValidProductWhenDeleteShouldRemoveProductFromCustomerBasket()
        {
            //Act
            var result = await underTest.Delete(UnitTestHelpers.CustomerId3, 10);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
