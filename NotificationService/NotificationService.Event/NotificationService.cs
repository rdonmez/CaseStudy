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
            _logger.LogInformation("NotificationService is starting.");

            // Subscribe to Order events.
            await _eventManager.SubscribeAsync<OrderEvent>(
                queueName: Constant.NotificationQueue,
                routingKey: Constant.OrderEventsRoutingKey,
                onMessage: async (orderEvent) =>
                {
                    _logger.LogInformation("Received OrderEvent event from NotificationService for Order Id: {OrderEventOrderId}", orderEvent.OrderId);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                        
                        var emailNotification = await repository.AddNotificationAsync(new Notification()
                        { 
                            CustomerEmail = orderEvent.CustomerEmail, 
                            Message = GetNotificationMessage(orderEvent),
                            Type = NotificationType.Email
                        });
                        
                        var smsNotification = await repository.AddNotificationAsync(new Notification()
                        { 
                            CustomerEmail = orderEvent.CustomerEmail, 
                            Message = GetNotificationMessage(orderEvent),
                            Type = NotificationType.Sms
                        });
                        
                        await Notify(emailNotification);
                        
                        await Notify(smsNotification);
                    }
                    

                    await Task.CompletedTask;
                }); 
 
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("NotificationService is stopping.");
        }
 
        private string GetNotificationMessage(OrderEvent orderEvent)
        {
            if (orderEvent.Status == OrderStatus.Created)
            {
                return $"Siparişiniz oluşturuldu. OrderId: {orderEvent.OrderId}. ";
            }
            else if (orderEvent.Status == OrderStatus.Approved)
            {
                return $"Siparişiniz onaylandı. OrderId: {orderEvent.OrderId}. ";
            }
            else if (orderEvent.Status == OrderStatus.Cancelled)
            {
                return $"Siparişiniz iptal edildi. OrderId: {orderEvent.OrderId}. ";
            }
            else if (orderEvent.Status == OrderStatus.Delivered)
            {
                return $"Siparişiniz teslim edildi. OrderId: {orderEvent.OrderId}. ";
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
         
        private async Task Notify(Notification notification)
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
                    await _eventManager.PublishAsync(notificationCreatedEvent, Constant.NotificationSendRoutingKey, Constant.NotificationSendQueue);
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred when publishing notification send. Please try again later.", ex);
            }
        }
    }
    
    
    
}