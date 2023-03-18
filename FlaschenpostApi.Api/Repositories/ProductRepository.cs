using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FlaschenpostApi.Models;

namespace FlaschenpostApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;

        public ProductRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string url)
        {
            var result = await _httpClient.GetAsync(url);
            var stringContent = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(stringContent);
            return products;
        }


        public async Task<IList<Product>> GetBierByPrice(string url)
        {
            var products = await GetProductsAsync(url);

            var filteredProducts = products.Select(p => new Product
            {
                Id = p.Id,
                BrandName = p.BrandName,
                Name = p.Name,
                DescriptionText = p.DescriptionText,
                Articles = p.Articles.Where(a => a.Price == 17.99)
                                                                  .Select(a => new Article
                                                                  {
                                                                      Id = a.Id,
                                                                      ShortDescription = a.ShortDescription,
                                                                      Price = a.Price,
                                                                      Unit = a.Unit,
                                                                      PricePerUnitText = a.PricePerUnitText,
                                                                      Image = a.Image
                                                                  })
                                                                  .ToList(),
               
            }).Where(p => p.Articles.Any());



            return filteredProducts.ToList();
        }

        public async Task<IList<Product>> GetExpensiveAndCheapestBier(string url)
        {

            var products = await GetProductsAsync(url);
            var articles = products
               .SelectMany(p => p.Articles)
               .Where(a => a.Unit == "Liter")
               .OrderByDescending(a => a.Price);

            var expensiveArticle = articles.First();
            var cheapestArticle = articles.Last();

            var expensiveProduct = products.Where(p => p.Articles.Contains(expensiveArticle)).First();
            var cheapestProduct = products.Where(p => p.Articles.Contains(cheapestArticle)).First();

            return new List<Product>() { new Product() { Id = expensiveProduct.Id,
                                                        BrandName = expensiveProduct.BrandName,
                                                        DescriptionText = expensiveProduct.DescriptionText,
                                                        Name = expensiveProduct.Name,
                                                        Articles = new List<Article>(){ expensiveArticle } } ,

            new Product() { Id = cheapestProduct.Id,
                                                        BrandName = cheapestProduct.BrandName,
                                                        DescriptionText = cheapestProduct.DescriptionText,
                                                        Name = cheapestProduct.Name,
                                                        Articles = new List<Article>(){ cheapestArticle }}
        };
        }



        public async Task<Product> GetProductWithMostBottles(string url)
        {
            var products = await GetProductsAsync(url);

            var productBottleCounts = new Dictionary<Product, int>();

            foreach (var product in products)
            {
                int totalBottles = 0;

                foreach (var article in product.Articles)
                {
                    // Parse the shortDescription string to get the number of bottles
                    if (int.TryParse(Regex.Match(article.ShortDescription, @"\d+").Value, out int bottleCount))
                    {
                        totalBottles += bottleCount;
                    }
                }

                productBottleCounts.Add(product, totalBottles);
            }

            // Sort the dictionary by descending order of bottle counts and return the product with the highest count
            return productBottleCounts.OrderByDescending(kvp => kvp.Value).First().Key;
        }

    }
}
