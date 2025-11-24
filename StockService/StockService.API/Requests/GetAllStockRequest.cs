#nullable enable
using System.Collections.Generic;
using MediatR;
using StockService.Entity.Entities;

namespace StockService.API.Requests
{
    public class GetAllStockRequest : IRequest<IEnumerable<Stock>?>
    { 
        
    }
}