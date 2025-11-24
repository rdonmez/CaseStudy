using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderService.API.Requests;
using OrderService.Entity.Domain;
using OrderService.Entity.Repository; 

namespace OrderService.API.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
    {
        private readonly IOrderRepository _repository;

        public CreateOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                CustomerId = request.CustomerId,
                Items = request.OrderItems,
                Address = request.Address,
                Status = OrderStatus.Waiting, 
                OrderDate = DateTime.UtcNow,
            };
        
            try
            { 
                return await _repository.AddOrderAsync(order); 
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving data. Please try again later.", ex);
            }
        }
    }
}