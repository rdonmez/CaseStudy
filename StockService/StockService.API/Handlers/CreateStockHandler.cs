#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR; 
using StockService.API.Requests;
using StockService.Entity;
using StockService.Entity.Entities;
using StockService.Entity.Repositories;

namespace StockService.API.Handlers
{
    public class CreateStockHandler : IRequestHandler<CreateStockRequest, Stock>
    {
        private readonly IStockRepository _repository;

        public CreateStockHandler(IStockRepository repository)
        {
            _repository = repository;
        }

        public async Task<Stock> Handle(CreateStockRequest request, CancellationToken cancellationToken)
        {
            var stock = new Stock
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = request.Price,
                CreatedAt = DateTime.UtcNow,
                Name = request.Name
            };
        
            try
            { 
                return await _repository.CreateAsync(stock); 
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving data. Please try again later.", ex);
            }
        }
    }
}