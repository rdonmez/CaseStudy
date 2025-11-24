#nullable enable
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderService.API.Requests;
using OrderService.Entity.Domain;
using OrderService.Entity.Repository;

namespace OrderService.API.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrderRequest, Order?>
    {
        private readonly IOrderRepository _repository;

        public GetOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order?> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetOrderByIdAsync(request.Id);
        }
    }
}