using Microsoft.AspNetCore.Mvc;
using SparkChange.Contracts;
using SparkChange.Resources;
using SparkChange.Resources.Validators.Exceptions;
using SparkChange.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace SparkChange.Controllers
{
    [ApiController]
    [Route("/api/goods")]
    [Produces("application/json")]
    public class GoodsController : Controller
    {
        private readonly IGoodsResource goodsResource;

        public GoodsController(IGoodsResource goodsResource)
        {
            this.goodsResource = goodsResource;
        }

        /// <summary>
        /// Allows an API consumer to retrieve all goods.
        /// </summary>
        /// <param name="currency">Goods currency by default USD.</param> 
        /// <returns>List of available products.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> List(CurrencyValue currency = ApplicationConstants.DefaultCurrency)
        {
            try
            {
                var result = await goodsResource.GetAll(currency);

                return Ok(result);
            }
            catch(ApiClientResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error happened during loading the list of products");
            }
        }
    }
}
