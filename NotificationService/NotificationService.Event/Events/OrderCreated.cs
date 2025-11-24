using System;
using System.Collections.Generic;

namespace NotificationService.Event.Events
{
    public class OrderCreated : ICloneable
    { 
        public int OrderId { get; set; }
         
        public int CustomerId { get; set; }
        
        public string CustomerEmail { get; set; }
    
        public object Clone()
        {
            return this.MemberwiseClone();
        } 
    }
}