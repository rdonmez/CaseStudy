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

namespace NotificationService.Event
{
    public class NotificationSendService : BackgroundService
    {  
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
                queueName: Constant.NotificationSendQueue,
                routingKey: Constant.NotificationSendRoutingKey,
                onMessage: async (notificationCreatedEvent) =>
                    {
                    _logger.LogInformation("Received NotificationCreated event from NotificationSendService for Order Id: {notificationCreatedEventId}", notificationCreatedEvent.Id);
 
                    using (var scope = _scopeFactory.CreateScope())
                    { 
                        var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                        var notification = await repository.GetNotificationByIdAsync(notificationCreatedEvent.Id);

                        switch (notification.Type)
                        {
                            case NotificationType.Email:
                                await SendEmailNotification(notification);
                                break;
                            case NotificationType.Sms:
                                await SendSmsNotification(notification);
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        
                        
                        notification.Status = NotificationStatus.Sent;
                        repository.UpdateNotification(notification);
                    }

                    _logger.LogInformation("Notification Sent for NotificationID: {notificationCreatedEventId}", notificationCreatedEvent.Id);

                    await Task.CompletedTask;
                });
             
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("NotificationSendService is stopping.");
        }

        private async Task SendEmailNotification(Notification notification)
        {
            // TODO: send email notification
             
        } 
        
        private async Task SendSmsNotification(Notification notification)
        {
            // TODO: send email notification
             
        } 
    }
}