using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FlaschenpostApi.Controllers;
using FlaschenpostApi.Models;
using FlaschenpostApi.Repositories;

namespace FlaschenpostApi.Tests
{
    public class Tests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductsController _productsController;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productsController = new ProductsController(_mockProductRepository.Object);
        }

        [Test]
        public async Task GetProducts_ReturnsOkResult_WhenProductsExist()
        {
            List<Product> expectedProducts = GetExpectedProducts();
            _mockProductRepository.Setup(s => s.GetProductsAsync("")).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productsController.GetAllProducts("");

            // Assert
            Assert.IsNotNull(result);


            CollectionAssert.AreEqual(expectedProducts, result);
        }

        private static List<Product> GetExpectedProducts()
        {
            // Arrange
            return new List<Product>()
            {
                new Product() { Id = 1, Name = "Product 1", Articles = new List<Article>(){
                    new Article(){Id = 111, Price = 20.99, Unit="Liter", PricePerUnitText = "(1.80 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                     new Article(){Id = 112, Price = 24.99,Unit="Liter", PricePerUnitText = "(2.50 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                } },
                new Product() { Id = 2, Name = "Product 2", Articles = new List<Article>(){
                    new Article(){Id = 113, Price = 17.99,Unit="Liter", PricePerUnitText = "(1.80 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                     new Article(){Id = 114, Price = 14.99,Unit="Liter", PricePerUnitText = "(1.50 €/Liter)", ShortDescription = "20 x 0,5L (Glas)"},
                } },
             new Product() { Id = 3, Name = "Product 3", Articles = new List<Article>(){
                    new Article(){Id = 115, Price = 20.99,Unit="Liter", PricePerUnitText = "(2.80 €/Liter)", ShortDescription = "24 x 0,5L (Glas)"},
                     new Article(){Id = 116, Price = 24.99, Unit="Liter",PricePerUnitText = "(2.50 €/Liter)", ShortDescription = "24 x 0,5L (Glas)"},
                } },
            };
        }



    }
}
