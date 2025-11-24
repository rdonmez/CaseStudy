

#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.API.Requests; 
using NotificationService.Entity.Domain;
using NotificationService.Entity.Repositories;

namespace NotificationService.API.Handlers
{
    public class SendNotificationHandler : IRequestHandler<SendNotificationRequest, Notification>
    {
        private ILogger<SendNotificationHandler> _logger;
        private readonly INotificationRepository _repository;
        //private readonly EventManager _eventManager;

        public SendNotificationHandler(INotificationRepository repository, ILogger<SendNotificationHandler> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Notification> Handle(SendNotificationRequest request, CancellationToken cancellationToken)
        {
            Notification? notification = new Notification();
            
            try
            {
                _logger.LogDebug($"Sending notification...");

                var id = await _repository.AddNotificationAsync(new Notification()
                {
                    Message = request.Message,
                    NotificationType = request.NotificationType,
                    CustomerId = request.CustomerId,
                    CustomerEmail = request.CustomerEmail,
                });
                
                // message gonderilirken create edilebilir. 
                // publish message
                
                _logger.LogDebug($"Publishing notification...");
    
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the notification. ", ex);
            } 
        }
    }
}