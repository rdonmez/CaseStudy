#nullable enable
using MediatR; 
using StockService.Entity.Entities;

namespace StockService.API.Requests
{
    public class GetStockRequest : IRequest<Stock?>
    {
        public int ProductId { get; set; }
    }
}