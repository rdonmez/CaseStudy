using System;
using System.Collections.Generic; 

namespace StockService.Event.Events
{
    public class OrderCreated
    {
        public int OrderId { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public int CustomerId { get; set; }   
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
    
    public class OrderItem
    {  
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
    }
}