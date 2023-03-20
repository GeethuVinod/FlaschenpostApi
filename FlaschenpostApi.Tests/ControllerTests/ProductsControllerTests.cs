using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FlaschenpostApi.Controllers;
using FlaschenpostApi.Models;
using FlaschenpostApi.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FlaschenpostApi.Api.Models;

namespace FlaschenpostApi.Tests
{
    public class Tests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductsController _productsController;

        private const string Url = "https://flapotest.blob.core.windows.net/test/ProductData.json";


        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productsController = new ProductsController(_mockProductRepository.Object);
        }

        [Test]
        public async Task GetProducts_ReturnsOkResult_IfUrlIsValid()
        {
            List<Product> expectedProducts = MockData.GetExpectedProducts();
            _mockProductRepository.Setup(s => s.GetProductsAsync(Url)).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productsController.GetAllProducts(Url);

            // Assert
            Assert.IsNotNull(result);

            CollectionAssert.AreEqual(expectedProducts, result);
        }

        [Test]
        public async Task GetBierByPrice_ReturnsOkResult_WhenUrlAndPriceIsValid()
        {
            var price = "17.99";
            var parsedPrice = 17.99;
            var expectedProducts = MockData.GetExpectedProducts().Where(p => p.Articles.Any(a => a.Price == parsedPrice)).ToList();
            // Act
            _mockProductRepository.Setup(repo => repo.GetBierByCost(Url, parsedPrice)).ReturnsAsync(new DomainResponse<IList<Product>>(expectedProducts));

            var response = await _productsController.GetBierByCost(Url, price);
            // Assert

            Assert.IsNotNull(response);
            
            Assert.IsInstanceOf<OkObjectResult>(response.Result);

            var actualProducts = (DomainResponse<IList<Product>>)((OkObjectResult)response.Result).Value;
            Assert.AreEqual(actualProducts.Result.Count(), expectedProducts.Count());
            Assert.AreEqual(actualProducts.Result.First().Id, expectedProducts.First().Id);

        }

        [Test]
        public async Task GetBierByPrice_ReturnsBadRequest_WhenPriceIsValid()
        {
            var price = "testprice";
            var parsedPrice = 17.99;
            var expectedProducts = MockData.GetExpectedProducts().Where(p => p.Articles.Any(a => a.Price == parsedPrice)).ToList();
            // Act
            _mockProductRepository.Setup(repo => repo.GetBierByCost(Url, parsedPrice)).ReturnsAsync(new DomainResponse<IList<Product>>(expectedProducts));

            var response = await _productsController.GetBierByCost(Url, price);
            // Assert

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);

        }

        [Test]
        public async Task GetBierByPrice_ReturnsBadRequest_WhenUrlIsValid()
        {
            var url = "";
            var expectedProducts = MockData.GetExpectedProducts();
            // Act
            _mockProductRepository.Setup(repo => repo.GetBierByCost(Url,17.99)).ReturnsAsync(new DomainResponse<IList<Product>>(expectedProducts));

            var response = await _productsController.GetBierByCost(url, "17.99");
            // Assert

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);

        }

    }
}
