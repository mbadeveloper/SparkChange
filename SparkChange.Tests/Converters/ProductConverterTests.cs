using FluentAssertions;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Converters;
using SparkChange.Tests.Helpers;
using SparkChange.Utilities;

namespace SparkChange.Tests.Converters
{
    [TestFixture]
    public class ProductConverterTests
    {
        private IProductConverter underTest;

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new ProductConverter();
        }

        [Test]
        public void WhenProductResponseConverterRunThenExpectFieldsMappedCorrectly()
        {
            var product = UnitTestHelpers.GetProduct43;
            var expectedResult = UnitTestHelpers.GetProductResponse43(ApplicationConstants.DefaultCurrency, 1);

            //Act
            var result = underTest.ToProductrResponse(product, ApplicationConstants.DefaultCurrency, 1);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GivenDifferentCurrencyWhenProductResponseConverterRunThenExpectFieldsMappedCorrectly()
        {
            var exchangeRate = 0.917M;
            var product = UnitTestHelpers.GetProduct43;
            var expectedResult = UnitTestHelpers.GetProductResponse43(CurrencyValue.BAM, exchangeRate);

            //Act
            var result = underTest.ToProductrResponse(product, CurrencyValue.BAM, exchangeRate);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
