#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Requests;
using OrderService.Entity.Domain;
using OrderService.Entity.Repository;
using OrderService.Event;
using OrderService.Event.Events;
using OrderService.Event.Exceptions;
using Polly;

namespace OrderService.API.Handlers
{
    public class ApproveOrderHandler : IRequestHandler<ApproveOrderRequest, Order?>
    {
        private readonly IOrderRepository _repository;
        private readonly EventManager _eventManager;
        
        public ApproveOrderHandler(IOrderRepository repository, EventManager eventManager)
        {
            _repository = repository;
            _eventManager = eventManager;
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

                await Notify(order);
                
                return order;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while changing status of the order. Please try again later.", ex);
            } 
        }
        
        private async Task Notify(Order order)
        {
            var orderApprovedEvent = new OrderApproved()
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                CustomerEmail = order.CustomerEmail,
                Total = order.Total,
                Items = order.Items,
                Status = OrderStatus.Approved,
            };
            
            var retryPolicy = Policy
                .Handle<MessageException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            
            await retryPolicy.ExecuteAsync(async () =>
            {
                await _eventManager.PublishAsync(orderApprovedEvent, Constant.OrderApprovedRoutingKey, new [] {Constant.NotificationQueue});
            });
        }
    }
}