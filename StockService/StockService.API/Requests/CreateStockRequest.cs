using MediatR; 
using StockService.Entity.Entities;

namespace StockService.API.Requests
{
    public class CreateStockRequest : IRequest<Stock>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
    }
}