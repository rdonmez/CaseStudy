using System;
using System.Collections.Generic;

namespace NotificationService.Event.Events
{
    public class OrderEvent
    { 
        public int OrderId { get; set; }
         
        public int CustomerId { get; set; }
        
        public string CustomerEmail { get; set; }
        
        public OrderStatus Status { get; set; }
     
    }

    public enum OrderStatus
    {
        Created,
        Approved,
        Shipped,
        Delivered,
        Cancelled
    }
}