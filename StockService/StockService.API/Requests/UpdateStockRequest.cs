#nullable enable
using MediatR;
using StockService.Entity.Entities;

namespace StockService.API.Requests
{
    public class UpdateStockRequest : IRequest<Stock?>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
    }
}