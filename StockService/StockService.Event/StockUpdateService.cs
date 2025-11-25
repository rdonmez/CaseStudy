using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 
using StockService.Entity.Repositories;
using StockService.Event.Events; 

namespace StockService.Event
{
    public class StockUpdateService : BackgroundService
    {
        private const string StockQueue = "stock_queue";
        private const string OrderCreatedRoutingKey = "order.created";
        
        private readonly ILogger<StockUpdateService> _logger;
        private readonly EventManager _eventManager;
        private readonly IServiceScopeFactory _scopeFactory;
        
        public StockUpdateService(ILogger<StockUpdateService> logger, EventManager eventManager,  IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _eventManager = eventManager; 
             _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("StockUpdate service is started.");

            await _eventManager.SubscribeAsync<OrderCreated>(
                queueName: StockQueue,
                routingKey: OrderCreatedRoutingKey,
                onMessage: async (orderCreatedEvent) =>
                {
                    _logger.LogInformation($"Received OrderCreated event - OrderId: {orderCreatedEvent.OrderId}");

                    foreach (var item in orderCreatedEvent.Items)
                    {
                        using var scope = _scopeFactory.CreateScope();
                       
                        var repository = scope.ServiceProvider.GetRequiredService<IStockRepository>();
                        var stock = await repository.GetByIdAsync(item.ProductId);

                        if (stock.Quantity < item.Quantity) continue;
                        // TODO: out of stock. rollback, cancel order and notify 
                        
                        stock.Quantity -= item.Quantity;
                        stock.UpdatedAt = DateTime.UtcNow;
                        repository.Update(stock);
                    }

                    await Task.CompletedTask;
                });

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("StockUpdate service is stopping.");
        }
    }
}