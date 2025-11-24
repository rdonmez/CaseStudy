using System;

namespace StockService.Event.Events
{ 
    public class StockUpdateEvent: ICloneable
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } 
        
        public DateTime UpdatedAt { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}