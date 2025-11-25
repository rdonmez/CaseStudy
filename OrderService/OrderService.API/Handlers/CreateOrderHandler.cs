using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderService.API.Requests;
using OrderService.Entity.Domain;
using OrderService.Entity.Repository;
using OrderService.Event;
using Polly;
using OrderService.Event.Events;
using OrderService.Event.Exceptions;

namespace OrderService.API.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
    {
        private readonly IOrderRepository _repository;
        private readonly EventManager _eventManager;
         
        public CreateOrderHandler(IOrderRepository repository, EventManager eventManager)
        {
            _repository = repository;
            _eventManager = eventManager;
        }

        public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                CustomerId = request.CustomerId,
                CustomerEmail = request.CustomerEmail,
                Address = request.Address,
                Items = request.OrderItems,
                Status = OrderStatus.Created, 
                OrderDate = DateTime.UtcNow,
            };
        
            try
            { 
                // TODO: Check stock if items are available. 
                
                order = await _repository.AddOrderAsync(order); 
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving data. Please try again later.", ex);
            }
            
            try
            {
                // Publish.
                await Notify(order);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while publishing the order. Please try again later.", ex);
            }

            return order;
        }

        private async Task Notify(Order order)
        {
            var orderCreatedEvent = new OrderCreated
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                CustomerEmail = order.CustomerEmail,
                Total = order.Total,
                Items = order.Items,
                Status = order.Status,
            };
            
            var retryPolicy = Policy
                .Handle<MessageException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            
            await retryPolicy.ExecuteAsync(async () =>
            {
                await _eventManager.PublishAsync(orderCreatedEvent, Constant.OrderCreatedRoutingKey, new [] {Constant.StockQueue, Constant.NotificationQueue});
            });
        }
    }
}