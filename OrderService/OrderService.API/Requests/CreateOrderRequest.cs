using System.Collections.Generic;
using MediatR;
using OrderService.Entity.Domain;

namespace OrderService.API.Requests
{
    public class CreateOrderRequest: IRequest<Order>
    {
        public int CustomerId { get; set; }
        
        public string CustomerEmail { get; set; }
        
        public string Address { get; set; }
        
        public List<OrderItem> OrderItems { get; set; }
    }
}