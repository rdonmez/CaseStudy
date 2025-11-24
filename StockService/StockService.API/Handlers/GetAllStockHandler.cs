#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StockService.API.Requests; 
using StockService.Entity.Entities;
using StockService.Entity.Repositories;

namespace StockService.API.Handlers
{
    public class GetAllStockHandler : IRequestHandler<GetAllStockRequest, IEnumerable<Stock>?>
    {
        private readonly IStockRepository _repository;

        public GetAllStockHandler(IStockRepository repository)
        {
            _repository = repository;
        }
  
        public async Task<IEnumerable<Stock>?> Handle(GetAllStockRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}