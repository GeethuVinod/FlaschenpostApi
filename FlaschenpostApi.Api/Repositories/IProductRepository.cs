using System.Collections.Generic;
using System.Threading.Tasks;
using FlaschenpostApi.Api.Models;
using FlaschenpostApi.Models;

namespace FlaschenpostApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(string url);
        Task<DomainResponse<IList<Product>>> GetExpensiveAndCheapestBier(string url);
        Task<DomainResponse<IList<Product>>> GetBierByCost(string url, double price);
        Task<DomainResponse<Product>> GetProductWithMostBottles(string url);
        Task<object> GetAll(string url);
    }
}
