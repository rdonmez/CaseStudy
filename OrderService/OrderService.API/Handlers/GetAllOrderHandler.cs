#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderService.API.Requests;
using OrderService.Entity.Domain;
using OrderService.Entity.Repository;

namespace OrderService.API.Handlers
{
    public class GetAllOrderHandler : IRequestHandler<GetAllOrderRequest, IEnumerable<Order>?>
    {
        private readonly IOrderRepository _repository;

        public GetAllOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>?> Handle(GetAllOrderRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllOrderAsync();
        }
    }
}