using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using FlaschenpostApi.Models;
using FlaschenpostApi.Repositories;

namespace FlaschenpostApi.Tests.RepositoryTests
{
    public class ProductRepositoryTests
    {
        private const string FlaschenpostProductsUrl = "https://flapotest.blob.core.windows.net/test/ProductData.json";

        private Mock<HttpMessageHandler> mockHttpHandler = new Mock<HttpMessageHandler>();

        private IProductRepository _productRepository;

        [SetUp]
        public void Setup()
        {
            // Arrange
            HttpClient httpClient = GetMockHttpClient();
            _productRepository = new ProductRepository(httpClient);
        }

        [Test]
        public async Task GetProductsAsync_ShouldReturnProducts_WhenUrlIsValid()
        {
            var expectedProducts = GetExpectedProducts();

            // Arrange

            var resultProducts = await _productRepository.GetProductsAsync(FlaschenpostProductsUrl);
            var actualProducts = resultProducts.ToList();

            // Assert
            Assert.NotNull(resultProducts);
            Assert.AreEqual(expectedProducts.Count, actualProducts.Count);
            for (var i = 0; i < expectedProducts.Count; i++)
            {
                Assert.AreEqual(expectedProducts[i].Name, actualProducts[i].Name);
                Assert.AreEqual(expectedProducts[i].Id, actualProducts[i].Id);

            }
        }

        [Test]
        public async Task GetBierByPrice1799_ReturnsFilteredProductsByPrice()
        {
            // Arrange

            var filteredProducts = (await _productRepository.GetBierByPrice(FlaschenpostProductsUrl)).ToList();

            // Assert
            Assert.NotNull(filteredProducts);
            Assert.AreEqual(2, filteredProducts.Count);
            Assert.AreEqual(2, filteredProducts.First().Id);
            Assert.AreEqual(3, filteredProducts[1].Id);
            Assert.AreEqual(2, filteredProducts.Where(p => p.Articles.Any(a => a.Price == 17.99)).Count());
        }

        [Test]
        public async Task GetExpensiveAndCheapestBier_ReturnsTwoProducts()
        {
            // Arrange

            var filteredProducts = (await _productRepository.GetExpensiveAndCheapestBier(FlaschenpostProductsUrl)).ToList();

            // Assert
            Assert.NotNull(filteredProducts);
            Assert.AreEqual(2, filteredProducts.Count);
            Assert.AreEqual(1, filteredProducts.First().Id);
            Assert.AreEqual(112, filteredProducts.First().Articles.First().Id);
            Assert.AreEqual(2, filteredProducts[1].Id);
            Assert.AreEqual(114, filteredProducts.First().Articles.First().Id);
        }





        private HttpClient GetMockHttpClient()
        {

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(GetExpectedProducts()))
            };

            // Act
            mockHttpHandler
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpHandler.Object);
            return httpClient;
        }

        private static List<Product> GetExpectedProducts()
        {
            // Arrange
            return new List<Product>()
            {
                new Product() { Id = 1, Name = "Product 1", Articles = new List<Article>(){
                    new Article(){Id = 111, Price = 20.99, Unit = "Liter", PricePerUnitText = "(1.80 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                     new Article(){Id = 112, Price = 24.99,Unit = "Liter", PricePerUnitText = "(2.50 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                } },
                new Product() { Id = 2, Name = "Product 2", Articles = new List<Article>(){
                    new Article(){Id = 113, Price = 17.99,Unit = "Liter", PricePerUnitText = "(1.80 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                     new Article(){Id = 114, Price = 14.99,Unit = "Liter", PricePerUnitText = "(1.50 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                } },
             new Product() { Id = 3, Name = "Product 3", Articles = new List<Article>(){
                    new Article(){Id = 115, Price = 20.99, Unit = "Liter",PricePerUnitText = "(2.80 €/Liter)", ShortDescription = "24 x 0,5L (Glas)"},
                     new Article(){Id = 116, Price = 17.99,Unit = "Liter", PricePerUnitText = "(1.50 €/Liter)", ShortDescription = "24 x 0,5L (Glas)"},
                } },
            };
        }

    }
}
