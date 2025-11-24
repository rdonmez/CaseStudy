#nullable enable
using System.Collections.Generic;
using MediatR;
using OrderService.Entity.Domain;

namespace OrderService.API.Requests
{
    public class GetAllOrderRequest: IRequest<IEnumerable<Order>?>
    {
        
    }
}