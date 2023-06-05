using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SparkChange.Contracts;
using SparkChange.Controllers;
using SparkChange.Resources;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Tests.Helpers;
using SparkChange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SparkChange.Tests.Controllers
{
    public class BasketControllerTests
    {
        private BasketController underTest;
        private readonly Mock<IBasketResource> mockBasketResource = new Mock<IBasketResource>();

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new BasketController(mockBasketResource.Object);
        }

        [TearDown]
        public void TearDown()
        {
            mockBasketResource.Invocations.Clear();
        }

        [Test]
        public async Task GivenCallsBasketResourceWithCorrectParametersWhenGetShouldSuccess()
        {
            mockBasketResource.Setup(x => x.Get(It.IsAny<Guid>(),It.IsAny<CurrencyValue>())).ReturnsAsync(new BasketResponse());

            var result = await underTest.Get(UnitTestHelpers.CustomerId1);

            mockBasketResource.Verify(gr => gr.Get(It.IsAny<Guid>(), It.IsAny<CurrencyValue>()), Times.Once);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task GivenValidationFailedWhenGetShouldThrowValidationException()
        {
            string exceptionMessage = "Validation exception message";

            mockBasketResource.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CurrencyValue>())).Throws(new ValidationException(exceptionMessage));

            var result = await underTest.Get(UnitTestHelpers.CustomerId2, ApplicationConstants.DefaultCurrency);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenApiCallFailedWhenGetShouldThrowApiClientResponseException()
        {
            string exceptionMessage = "Exception message";

            mockBasketResource.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CurrencyValue>())).Throws(new ApiClientResponseException(exceptionMessage, StatusCodes.Status404NotFound));

            var result = await underTest.Get(UnitTestHelpers.CustomerId2, ApplicationConstants.DefaultCurrency);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenFailedWhenListShouldThrowException()
        {
            mockBasketResource.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CurrencyValue>())).Throws(new Exception());

            var result = await underTest.Get(UnitTestHelpers.CustomerId2, ApplicationConstants.DefaultCurrency);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("Failed to return customer shopping basket", ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenCallsBasketResourceWithCorrectParametersWhenPostShouldSuccess()
        {
            mockBasketResource.Setup(x => x.Post(It.IsAny<Guid>(), It.IsAny<BasketItemRequest>())).ReturnsAsync(new BasketItemResponse());

            var result = await underTest.Post(UnitTestHelpers.GetBasketItemRequest, UnitTestHelpers.CustomerId1);

            mockBasketResource.Verify(gr => gr.Post(It.IsAny<Guid>(), It.IsAny<BasketItemRequest>()), Times.Once);

            Assert.IsInstanceOf<CreatedResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)result.Result).StatusCode);
            Assert.IsInstanceOf<BasketItemResponse>(((CreatedResult)result.Result).Value);
        }

        [Test]
        public async Task GivenValidationFailedWhenPostShouldThrowValidationException()
        {
            string exceptionMessage = "Validation exception message";

            mockBasketResource.Setup(x => x.Post(It.IsAny<Guid>(), It.IsAny<BasketItemRequest>())).Throws(new ValidationException(exceptionMessage));

            var result = await underTest.Post(UnitTestHelpers.GetBasketItemRequest, UnitTestHelpers.CustomerId1);

            Assert.IsInstanceOf<ObjectResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result.Result).Value);
        }

        [Test]
        public async Task GivenFailedWhenPostShouldThrowException()
        {
            mockBasketResource.Setup(x => x.Post(It.IsAny<Guid>(), It.IsAny<BasketItemRequest>())).Throws(new Exception());

            var result = await underTest.Post(UnitTestHelpers.GetBasketItemRequest, UnitTestHelpers.CustomerId1);

            Assert.IsInstanceOf<ObjectResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
            Assert.AreEqual("Failed to add the product to the customer shopping basket", ((ObjectResult)result.Result).Value);
        }

        [Test]
        public async Task GivenCallsBasketResourceWithCorrectParametersWhenDeleteShouldSuccess()
        {
            mockBasketResource.Setup(x => x.Delete(It.IsAny<Guid>(), It.IsAny<int>()));

            var result = await underTest.Delete(4, UnitTestHelpers.CustomerId3);

            mockBasketResource.Verify(gr => gr.Delete(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GivenValidationFailedWhenDeleteShouldThrowValidationException()
        {
            string exceptionMessage = "Validation exception message";

            mockBasketResource.Setup(x => x.Delete(It.IsAny<Guid>(), It.IsAny<int>())).Throws(new ValidationException(exceptionMessage));

            var result = await underTest.Delete(5, UnitTestHelpers.CustomerId2);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenApiCallFailedWhenDeleteShouldThrowApiClientResponseException()
        {
            string exceptionMessage = "Exception message";

            mockBasketResource.Setup(x => x.Delete(It.IsAny<Guid>(), It.IsAny<int>())).Throws(new ApiClientResponseException(exceptionMessage, StatusCodes.Status404NotFound));

            var result = await underTest.Delete(3, UnitTestHelpers.CustomerId2);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenFailedWhenDeleteShouldThrowException()
        {
            mockBasketResource.Setup(x => x.Delete(It.IsAny<Guid>(), It.IsAny<int>())).Throws(new Exception());

            var result = await underTest.Delete(7, UnitTestHelpers.CustomerId3);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("Failed to delete the product from the customer shopping basket", ((ObjectResult)result).Value);
        }
    }
}
