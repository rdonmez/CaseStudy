using MediatR;
using OrderService.Entity.Domain;

namespace OrderService.API.Requests
{
    public class CancelOrderRequest: IRequest<Order>
    {
        public int OrderId { get; set; }
    }
}