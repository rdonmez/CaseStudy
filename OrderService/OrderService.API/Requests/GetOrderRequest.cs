#nullable enable
using MediatR;
using OrderService.Entity.Domain;

namespace OrderService.API.Requests
{
    public class GetOrderRequest : IRequest<Order?>
    {
        public int Id { get; set; }
    }
}