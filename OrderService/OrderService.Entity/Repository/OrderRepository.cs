using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity.Domain;

namespace OrderService.Entity.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }
        
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o=> o.Items).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllOrderAsync()
        {
            return await _context.Orders.Include(o=> o.Items).OrderByDescending(t=> t.OrderDate).ToListAsync();
        }

        public async Task<Order> AddOrderAsync(Order order)
        { 
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }
}