#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR; 
using StockService.API.Requests; 
using StockService.Entity.Entities;
using StockService.Entity.Repositories;

namespace StockService.API.Handlers
{
    public class UpdateStockHandler : IRequestHandler<UpdateStockRequest, Stock?>
    {
        private readonly IStockRepository _repository;

        public UpdateStockHandler(IStockRepository repository)
        {
            _repository = repository;
        }

        public async Task<Stock?> Handle(UpdateStockRequest request, CancellationToken cancellationToken)
        {
            var stock = new Stock
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = request.Price,
                UpdatedAt = DateTime.UtcNow,
                Name = request.Name
            };
        
            try
            { 
                _repository.Update(stock);

                return stock;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving data. Please try again later.", ex);
            }
        }

         
    }
}