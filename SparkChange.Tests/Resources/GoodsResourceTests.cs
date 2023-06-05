using FluentAssertions;
using Moq;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Converters;
using SparkChange.Resources;
using SparkChange.Tests.Helpers;
using SparkChange.Utilities;

namespace SparkChange.Tests.Resources
{
    [TestFixture]
    public class GoodsResourceTests : AbstractDbTests
    {
        private IGoodsResource underTest;
        private readonly Mock<ICurrencyResource> mockCurrencyResource = new Mock<ICurrencyResource>();
        private readonly IProductConverter productConverter = new ProductConverter();

       [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new GoodsResource(databaseContext, mockCurrencyResource.Object, productConverter);
        }

        [Test]
        public async Task WhenGetAllShouldReturnListOfGoods()
        {
            var expectedProducts = UnitTestHelpers.GetProductsResponse(ApplicationConstants.DefaultCurrency, 1M);

            //Act
            var result = await underTest.GetAll(ApplicationConstants.DefaultCurrency);

            //Assert
            Assert.IsNotNull(result);
            result.Should().BeEquivalentTo(expectedProducts);
        }

        [Test]
        public async Task GivenDifferentCurrencyWhenGetAllShouldReturnGoodsInNewCurrency()
        {
            var exchangeRate = 0.786M;
            var expectedProducts = UnitTestHelpers.GetProductsResponse(CurrencyValue.CHF, exchangeRate);

            //Act
            mockCurrencyResource.Setup(c => c.GetExchangeRate(ApplicationConstants.DefaultCurrency, CurrencyValue.CHF)).ReturnsAsync(exchangeRate);
            var result = await underTest.GetAll(CurrencyValue.CHF);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            result.Should().BeEquivalentTo(expectedProducts);
        }
    }
}