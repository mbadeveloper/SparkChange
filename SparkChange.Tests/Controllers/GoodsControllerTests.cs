using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Controllers;
using SparkChange.Resources;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Utilities;

namespace SparkChange.Tests.Controllers
{
    [TestFixture]
    public class GoodsControllerTests
    {
        private GoodsController underTest;
        private readonly Mock<IGoodsResource> mockGoodsResource = new Mock<IGoodsResource>();

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new GoodsController(mockGoodsResource.Object);            
        }

        [TearDown]
        public void TearDown()
        {
            mockGoodsResource.Invocations.Clear();
        }

        [Test]
        public async Task GivenCallsGoodsResourceWithCorrectParametersWhenListShouldSuccess()
        {
            mockGoodsResource.Setup(x => x.GetAll(It.IsAny<CurrencyValue>())).ReturnsAsync(new List<ProductResponse>());

            var result = await underTest.List(ApplicationConstants.DefaultCurrency);

            mockGoodsResource.Verify(gr => gr.GetAll(It.IsAny<CurrencyValue>()), Times.Once);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task GivenApiCallFailedWhenListShouldThrowApiClientResponseException()
        {
            string exceptionMessage = "Exception message";

            mockGoodsResource.Setup(x => x.GetAll(It.IsAny<CurrencyValue>())).Throws(new ApiClientResponseException(exceptionMessage, 404));

            var result = await underTest.List(ApplicationConstants.DefaultCurrency);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenFailedWhenListShouldThrowException()
        {
            mockGoodsResource.Setup(x => x.GetAll(It.IsAny<CurrencyValue>())).Throws(new Exception());

            var result = await underTest.List(ApplicationConstants.DefaultCurrency);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("Unexpected error happened during loading the list of products", ((ObjectResult)result).Value);
        }
    }
}
