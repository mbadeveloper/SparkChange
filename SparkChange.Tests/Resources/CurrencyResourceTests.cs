using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Resources;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SparkChange.Tests.Resources
{
    [TestFixture]
    public class CurrencyResourceTests
    {
        private ICurrencyResource underTest;
        private readonly Mock<IApiClient> mockApiClient = new Mock<IApiClient>();
        private readonly Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new CurrencyResource(mockApiClient.Object, mockConfiguration.Object);

            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { });
            mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(mockSection.Object);
        }

        [Test]
        public async Task GivenCallCurrencyLayerSuccessWhenGetExchangeRateShouldReturnExchangeRate()
        {
            var exchangeRate = 0.657M;

            var currencyLayerResponse = new CurrencyLayerResponse
            {
                Success = true,
                Source = ApplicationConstants.DefaultCurrency,
                Quotes = new Dictionary<string, decimal>
                {
                    { $"{ApplicationConstants.DefaultCurrency}YER", exchangeRate}
                }
            };

            mockApiClient.Setup(x => x.GetRequest<CurrencyLayerResponse>(It.IsAny<string>(), null)).ReturnsAsync(currencyLayerResponse);
            
            var result = await underTest.GetExchangeRate(ApplicationConstants.DefaultCurrency, CurrencyValue.YER);

            Assert.IsNotNull(result);
            Assert.AreEqual(exchangeRate, result);
        }

        [Test]
        public async Task GivenCallCurrencyLayerNotReturnCurrencyPairWhenGetExchangeRateShouldThrowApiClientResponseException()
        {
            var exchangeRate = 0.657M;

            var currencyLayerResponse = new CurrencyLayerResponse
            {
                Success = true,
                Source = ApplicationConstants.DefaultCurrency,
                Quotes = new Dictionary<string, decimal>
                {
                    { "AAAYER", exchangeRate}
                }
            };

            mockApiClient.Setup(x => x.GetRequest<CurrencyLayerResponse>(It.IsAny<string>(), null)).ReturnsAsync(currencyLayerResponse);

            AsyncTestDelegate action = async () => { await underTest.GetExchangeRate(ApplicationConstants.DefaultCurrency, CurrencyValue.YER); };

            var exception = Assert.ThrowsAsync<ApiClientResponseException>(action);
            exception.Message.Should().Be("Exchange rate for currency USD to YER not found");
        }

        [Test]
        public async Task GivenCallCurrencyLayerFailWhenGetExchangeRateShouldThrowException()
        {
            var exchangeRate = 0.657M;

            var currencyLayerResponse = new CurrencyLayerResponse
            {
                Success = true,
                Source = ApplicationConstants.DefaultCurrency,
                Quotes = new Dictionary<string, decimal>
                {
                    { $"{ApplicationConstants.DefaultCurrency}YER", exchangeRate}
                }
            };

            mockApiClient.Setup(x => x.GetRequest<CurrencyLayerResponse>(It.IsAny<string>(), null)).ThrowsAsync(new ApiClientResponseException("Error response status returned", StatusCodes.Status102Processing));

            AsyncTestDelegate action = async () => { await underTest.GetExchangeRate(ApplicationConstants.DefaultCurrency, CurrencyValue.YER); };

            var exception = Assert.ThrowsAsync<ApiClientResponseException>(action);
            exception.Message.Should().Be("Error response status returned");
            Assert.AreEqual(StatusCodes.Status102Processing, exception.StatusCode);
        }
    }
}
