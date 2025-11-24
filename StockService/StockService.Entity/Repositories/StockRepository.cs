using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockService.Entity.Entities;

namespace StockService.Entity.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _context; 

        public StockRepository(StockDbContext context)
        {
            _context = context;
        }
        
        public async Task<Stock> GetByIdAsync(int productId)
        {
            return await _context.Stocks.FirstOrDefaultAsync(o => o.ProductId == productId);
        }

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync(); 
            return stock;
        }
          
        public void Update(Stock stock)
        {
            _context.Stocks.Update(stock);
            _context.SaveChanges();
        }
 
        public void Delete(Stock stock)
        {
            _context.Stocks.Remove(stock);
            _context.SaveChanges();
        }
    }
}