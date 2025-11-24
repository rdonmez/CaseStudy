using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Entity.Domain;

namespace OrderService.Entity.Repository
{
    public interface IOrderRepository
    { 
        Task<Order> GetOrderByIdAsync(int id);
 
        Task<IEnumerable<Order>> GetAllOrderAsync();
 
        Task<Order> AddOrderAsync(Order order);

        void UpdateOrder(Order order);
 
        void DeleteOrder(Order order);
    }
}