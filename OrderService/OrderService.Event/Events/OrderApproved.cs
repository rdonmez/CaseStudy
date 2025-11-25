using System;
using System.Collections.Generic;
using OrderService.Entity.Domain;

namespace OrderService.Event.Events
{
    public class OrderApproved
    {
        public int OrderId { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public int CustomerId { get; set; }
        
        public string CustomerEmail { get; set; }
 
        public OrderStatus Status { get; set; }
        
        public decimal Total  { get; set; }
        
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}

