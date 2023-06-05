using Microsoft.AspNetCore.Mvc;
using SparkChange.Contracts;
using SparkChange.Resources;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Utilities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SparkChange.Controllers
{
    [ApiController]
    [Route("/api/basket")]
    [Produces("application/json")]
    public class BasketController : Controller
    {
        private readonly IBasketResource basketResource;

        public BasketController(IBasketResource basketResource)
        {
            this.basketResource = basketResource;
        }

        // I don't included unathorised and forbidden because I don't implement authorisation process as is out scope of the assigment

        /// <summary>
        /// Allows an API consumer to retrieve customer basket.
        /// </summary>
        /// <param name="customerId">Customer Id.</param>
        /// <param name="currency">Basket currency by default USD.</param> 
        /// <returns>Basket items and total in the selected currency.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get(Guid customerId, CurrencyValue currency = ApplicationConstants.DefaultCurrency)
        {
            try
            {
                var result = await basketResource.Get(customerId, currency);

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ApiClientResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to return customer shopping basket");
            }
        }

        /// <summary>
        /// Allows an API consumer to add product to customer basket.
        /// </summary>
        /// <param name="customerId">Customer Id.</param>
        /// <param name="BasketItemRequest">Basket Item Request</param>
        /// <returns>BasketItemResponse</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BasketItemResponse>> Post(BasketItemRequest basketItem, Guid customerId)
        {
            try
            {
                var result = await basketResource.Post(customerId, basketItem);

                //I leave the Uri as empty string as there are no end point to get basket item in this example
                return Created(string.Empty,result);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the product to the customer shopping basket");
            }
        }

        /// <summary>
        /// Deletes an existing basket item for the given customer.
        /// </summary>
        /// <param name="customerId">Customer Id.</param>
        /// <param name="productId">Product Id.</param>        
        [HttpDelete]
        [SwaggerOperation(OperationId = "Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Delete(int productId, Guid customerId)
        {
            try
            {
                await basketResource.Delete(customerId, productId);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch(ApiClientResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the product from the customer shopping basket");
            }
        }
    }
}
