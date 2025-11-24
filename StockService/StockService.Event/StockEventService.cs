using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;  
using StockService.Entity;
using StockService.Entity.Repositories;
using StockService.Event.Events;
using StockService.Event.Exceptions;

namespace StockService.Event
{
    public class StockEventService : BackgroundService
    {
        private const string StockQueue = "stock_update_queue";
        private const string StockQueueUpdated = "stock_updated_queue";
        private const string StockUpdatedRoutingKey = "stock_updated_routing_key";

        private readonly ILogger<StockEventService> _logger;
        private readonly EventManager _eventManager;
        private readonly IStockRepository _repository;

        public StockEventService(ILogger<StockEventService> logger, EventManager eventManager)
        {
            _logger = logger;
            _eventManager = eventManager; 
             
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stock event service is started.");

            await _eventManager.SubscribeAsync<StockUpdateEvent>(
                queueName: StockQueue,
                routingKey: StockUpdatedRoutingKey,
                onMessage: async (stockUpdateEvent) =>
                {
                    _logger.LogInformation($"Received StockUpdateEvent - ProductId: {stockUpdateEvent.ProductId}");

                    var stock = await _repository.GetByIdAsync(stockUpdateEvent.ProductId);

                    stock.Quantity -= stockUpdateEvent.Quantity;
                    stock.UpdatedAt = DateTime.UtcNow;

                    _repository.Update(stock);

                    var retryPolicy = Policy
                        .Handle<MessageException>()
                        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                    await retryPolicy.ExecuteAsync(async () =>
                    {
                        await _eventManager.PublishAsync(stockUpdateEvent.Clone(), StockUpdatedRoutingKey, StockQueueUpdated);
                    });

                    await Task.CompletedTask;
                });

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Stock event service is stopping.");
        }
    }
}