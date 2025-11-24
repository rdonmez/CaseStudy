using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Entity;
using NotificationService.Entity.Domain;
using NotificationService.Entity.Repositories;
using NotificationService.Event.Events; 

namespace NotificationService.Event
{
    public class NotificationSendService : BackgroundService
    { 
        private const string NotificationQueue = "notification_queue";
        private const string NotificationSendRoutingKey = "notification.send";

        private readonly ILogger<NotificationSendService> _logger;
        private readonly EventManager _eventManager; 
        private readonly IServiceScopeFactory _scopeFactory;
        public NotificationSendService(ILogger<NotificationSendService> logger,
            EventManager eventManager,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _eventManager = eventManager; 
            _scopeFactory =  scopeFactory; 
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationSendService is starting.");

            // Subscribe.
            await _eventManager.SubscribeAsync<NotificationCreated>(
                queueName: NotificationQueue,
                routingKey: NotificationSendRoutingKey,
                onMessage: async (notificationCreatedEvent) =>
                    {
                    _logger.LogInformation($"Received NotificationCreated event for Order Id: {notificationCreatedEvent.Id}");
 
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        
                        var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                        var notification = await repository.GetNotificationByIdAsync(notificationCreatedEvent.Id);
                        
                        // TODO: SEND notification
                        
                        notification.Status = NotificationStatus.Sent;
                        repository.UpdateNotification(notification);
                    }

                    _logger.LogInformation($"Sent Notification for ID: {notificationCreatedEvent.Id}");

                    await Task.CompletedTask;
                });
             
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("NotificationSendService  is stopping.");
        }
    }
}