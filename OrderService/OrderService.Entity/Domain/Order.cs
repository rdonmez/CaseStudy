using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OrderService.Entity.Domain
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public int CustomerId { get; set; }
 
        public OrderStatus Status { get; set; }
    
        public string Address { get; set; }
    
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    
        public decimal Total
        {
            get
            {
                return Items.Select(x=> x.Quantity * x.UnitPrice).Sum();
            }
        }
    }
}