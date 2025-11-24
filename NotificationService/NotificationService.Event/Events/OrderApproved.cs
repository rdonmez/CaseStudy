using System;

namespace NotificationService.Event.Events
{
    public class OrderApproved: ICloneable
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