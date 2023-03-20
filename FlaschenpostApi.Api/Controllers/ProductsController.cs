using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlaschenpostApi.Models;
using FlaschenpostApi.Repositories;
using System;
using Microsoft.Extensions.Logging;

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

        [HttpGet("expensive-cheapest-per-litre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetMostExpensiveAndCheapestBierPerLitre([FromQuery] string url)
        {
            if(!ValidateUrl(url))
            {
                return BadRequest("Invalid URL format");
            }
           
            var products = await _productRepository.GetExpensiveAndCheapestBier(url);
           
            return Ok(products);
        }

        [HttpGet("cost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetBierByCost([FromQuery] string url, string price)
        {
            if (!ValidateUrl(url))
            {
                return BadRequest("Invalid URL format");
            }

            if (!double.TryParse(price, out double parsedPrice))
            {
                return BadRequest("Invalid price value.");
            }

            var filteredProducts = await _productRepository.GetBierByCost(url, parsedPrice);

            return Ok(filteredProducts);
        }

        [HttpGet("mostbottles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductWithMostBottles([FromQuery] string url)
        {
            if (!ValidateUrl(url))
            {
                return BadRequest("Invalid URL format");
            }

            var productWithMostBottles = await _productRepository.GetProductWithMostBottles(url);

            return Ok(productWithMostBottles);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQueries([FromQuery] string url)
        {
            if (!ValidateUrl(url))
            {
                return BadRequest("Invalid URL format");
            }
            var response = await _productRepository.GetAll(url);

            return Ok(response);
        }

        private bool ValidateUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uri);
        }
    }
}
