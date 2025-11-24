using System.ComponentModel.DataAnnotations;

namespace OrderService.Entity.Domain
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}