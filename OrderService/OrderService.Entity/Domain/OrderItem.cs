using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderService.Entity.Domain
{
    public class OrderItem
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}