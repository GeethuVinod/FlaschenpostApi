using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlaschenpostApi.Models;
using FlaschenpostApi.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlaschenpostApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllProducts([FromQuery] string url)
        {
            var products = await _productRepository.GetProductsAsync(url);
            return products;
        }

        [HttpGet("most-expensive-cheapest-per-litre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetMostExpensiveAndCheapestPerLitre([FromQuery] string url)
        {
            var products = await _productRepository.GetExpensiveAndCheapestBier(url);
           
            return Ok(products);
        }

        [HttpGet("cost-exactly-1799")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetBeersCostingExactly1799([FromQuery] string url)
        {
            var beersCosting1799 = await _productRepository.GetBierByPrice(url);

            return Ok(beersCosting1799);
        }

        [HttpGet("mostbottles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductWithMostBottles([FromQuery] string url)
        {
            var productWithMostBottles = await _productRepository.GetProductWithMostBottles(url);

            return Ok(productWithMostBottles);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQueries([FromQuery] string url)
        {
            var mostExpensiveAndCheapestPerLitre = await GetMostExpensiveAndCheapestPerLitre(url);
            var beersCosting1799 = await GetBeersCostingExactly1799(url);
            var productWithMostBottles = await GetProductWithMostBottles(url);

            var result = new
            {
                MostExpensiveAndCheapestPerLitre = ((OkObjectResult)mostExpensiveAndCheapestPerLitre.Result).Value,
                BeersCosting1799 = ((OkObjectResult)beersCosting1799.Result).Value,
                ProductWithMostBottles = ((OkObjectResult)productWithMostBottles.Result).Value
            };

            return Ok(result);
        }
    }
}
