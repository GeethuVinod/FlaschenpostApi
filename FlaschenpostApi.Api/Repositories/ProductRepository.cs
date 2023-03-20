using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FlaschenpostApi.Models;
using System;
using FlaschenpostApi.Api.Models;

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
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(await result.Content.ReadAsStringAsync());
        }

        public async Task<DomainResponse<IList<Product>>> GetBierByCost(string url, double price)
        {
            try
            {
                var products = await GetProductsAsync(url);

                var filteredProducts = products.Select(p => new Product
                {
                    Id = p.Id,
                    BrandName = p.BrandName,
                    Name = p.Name,
                    DescriptionText = p.DescriptionText,
                    Articles = p.Articles.Where(a => a.Price == price)
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

                }).Where(p => p.Articles.Any())
                  .ToList();

                return new DomainResponse<IList<Product>>(filteredProducts);
            }
            catch (Exception ex)
            {
                return new DomainResponse<IList<Product>>(null, ex.Message);
            }

        }

        public async Task<DomainResponse<IList<Product>>> GetExpensiveAndCheapestBier(string url)
        {
            try
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

                var filteredProducts = new List<Product>() { new Product() { Id = expensiveProduct.Id,
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
                return new DomainResponse<IList<Product>>(filteredProducts);
            }
            catch (Exception ex)
            {
                return new DomainResponse<IList<Product>>(null, ex.Message);
            }

        }

        public async Task<DomainResponse<Product>> GetProductWithMostBottles(string url)
        {
            try
            {
                var products = await GetProductsAsync(url);

                var productBottleCounts = new Dictionary<Product, int>();

                foreach (var product in products)
                {
                    int totalBottles = 0;

                    foreach (var article in product.Articles)
                    {
                        if (int.TryParse(Regex.Match(article.ShortDescription, @"\d+").Value, out int bottleCount))
                        {
                            totalBottles += bottleCount;
                        }
                    }

                    productBottleCounts.Add(product, totalBottles);
                }

                return new DomainResponse<Product>(productBottleCounts.OrderByDescending(kvp => kvp.Value)?.First().Key);
            }
            catch(Exception ex)
            {
                return new DomainResponse<Product>(null, ex.Message);
            }
           
        }

        public async Task<object> GetAll(string url)
        {
            return new
            {
                MostExpensiveAndCheapestPerLitre = await GetExpensiveAndCheapestBier(url),
                BeersCosting1799 = await GetBierByCost(url, 17.99),
                ProductWithMostBottles = await GetProductWithMostBottles(url)
            };

        }


    }
}
