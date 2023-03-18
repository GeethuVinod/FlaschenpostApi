using System.Collections.Generic;
using System.Threading.Tasks;
using FlaschenpostApi.Models;

namespace FlaschenpostApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(string url);
        Task<IList<Product>> GetExpensiveAndCheapestBier(string url);
        Task<IList<Product>> GetBierByPrice(string url);
        Task<Product> GetProductWithMostBottles(string url);
    }
}
