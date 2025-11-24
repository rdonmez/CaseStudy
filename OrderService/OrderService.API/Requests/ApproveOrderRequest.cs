#nullable enable
using System;
using MediatR;
using OrderService.Entity.Domain;

namespace OrderService.API.Requests
{
    public class ApproveOrderRequest: IRequest<Order?>
    {
        public int OrderId { get; set; }
    }
}