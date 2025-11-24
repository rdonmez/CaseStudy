using System.Collections.Generic;
using System.Threading.Tasks;
using StockService.Entity.Entities;

namespace StockService.Entity.Repositories
{
    public interface IStockRepository
    {
        Task<Stock> GetByIdAsync(int productId);
        
        Task<IEnumerable<Stock>> GetAllAsync();
        
        Task<Stock> CreateAsync(Stock stock);
          
        void Update(Stock stock);

        void Delete(Stock stock);
    }
}