

#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.API.Requests; 
using NotificationService.Entity.Domain;
using NotificationService.Entity.Repositories;
using NotificationService.Event;
using NotificationService.Event.Events;
using NotificationService.Event.Exceptions;
using Polly;

namespace NotificationService.API.Handlers
{
    public class SendNotificationHandler : IRequestHandler<SendNotificationRequest, Notification>
    {
        private ILogger<SendNotificationHandler> _logger;
        private readonly INotificationRepository _repository;
        private readonly EventManager _eventManager;

        private const string NotificationQueue = "notification_queue";
        private const string NotificationSendRoutingKey = "notification.send";
        
        public SendNotificationHandler(INotificationRepository repository, ILogger<SendNotificationHandler> logger, EventManager eventManager)
        {
            _logger = logger;
            _repository = repository;
            _eventManager = eventManager;
        }

        public async Task<Notification> Handle(SendNotificationRequest request, CancellationToken cancellationToken)
        { 
            try
            {
                _logger.LogDebug($"Sending notification...");

                var notification = await _repository.AddNotificationAsync(new Notification()
                {
                    OrderId = request.OrderId,
                    Message = request.Message,
                    NotificationType = request.NotificationType,
                    CustomerId = request.CustomerId,
                    CustomerEmail = request.CustomerEmail,
                });

                _logger.LogDebug($"Publishing notification...");
                
                await Notify(notification);
                 
                return notification;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the notification. ", ex);
            } 
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