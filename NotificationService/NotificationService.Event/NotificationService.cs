using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Entity;
using NotificationService.Entity.Domain;
using NotificationService.Entity.Repositories;
using NotificationService.Event.Events;
using NotificationService.Event.Exceptions;
using Polly;

namespace NotificationService.Event
{
    public class NotificationService : BackgroundService
    {  
        private const string NotificationQueue = "notification_queue";
        private const string OrderCreatedRoutingKey = "order.created";
        private const string NotificationSendRoutingKey = "notification.send";
        
        private readonly ILogger<NotificationService> _logger;
        private readonly EventManager _eventManager; 
        private readonly IServiceScopeFactory _scopeFactory;
        public NotificationService(ILogger<NotificationService> logger,
            EventManager eventManager,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _eventManager = eventManager; 
            _scopeFactory =  scopeFactory; 
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationService  is starting.");

            // Subscribe.
            await _eventManager.SubscribeAsync<OrderCreated>(
                queueName: NotificationQueue,
                routingKey: OrderCreatedRoutingKey,
                onMessage: async (orderEvent) =>
                {
                    _logger.LogInformation($"Received OrderCreated event for Order Id: {orderEvent.OrderId}");

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                        var notification = await repository.AddNotificationAsync(new Notification()
                        {
                            OrderId = orderEvent.OrderId,
                            CustomerEmail = orderEvent.CustomerEmail,
                            CustomerId = orderEvent.CustomerId,
                            Message = "Siparişiniz işleniyor...",
                            NotificationType = NotificationType.OrderCreated
                        });
                        
                        await Notify(notification);
                    }
                    

                    await Task.CompletedTask;
                }); 
 
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Notification background service is stopping.");
        }
        
        async Task Notify(Notification notification)
        {
            try
            { 
                var notificationCreatedEvent = new NotificationCreated
                {
                    Id =  notification.Id
                };
            
                var retryPolicy = Policy
                    .Handle<MessageException>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            
                await retryPolicy.ExecuteAsync(async () =>
                {
                    await _eventManager.PublishAsync(notificationCreatedEvent, NotificationSendRoutingKey, NotificationQueue);
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred when publishing notification send. Please try again later.", ex);
            }
        }
    }
    
    
    
}