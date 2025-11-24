#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Requests;
using OrderService.Entity.Domain;
using OrderService.Entity.Repository;

namespace OrderService.API.Handlers
{
    public class ApproveOrderHandler : IRequestHandler<ApproveOrderRequest, Order?>
    {
        private readonly IOrderRepository _repository;

        public ApproveOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order?> Handle(ApproveOrderRequest request, CancellationToken cancellationToken)
        {
            Order? order;

            try
            {
                order = await _repository.GetOrderByIdAsync(request.OrderId);

                if (order == null)
                {
                    return null;
                }

                order.Status = OrderStatus.Approved;

                _repository.UpdateOrder(order);

                // TODO: Publish OrderApproved event.
                
                return order;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the order. Please try again later.", ex);
            } 
        }
    }
}