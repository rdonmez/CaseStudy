#nullable enable
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StockService.API.Requests; 
using StockService.Entity.Entities;
using StockService.Entity.Repositories;

namespace StockService.API.Handlers
{
    public class GetStockHandler : IRequestHandler<GetStockRequest, Stock?>
    {
        private readonly IStockRepository _repository;

        public GetStockHandler(IStockRepository repository)
        {
            _repository = repository;
        }

        public async Task<Stock?> Handle(GetStockRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.ProductId);
        }
    }
}