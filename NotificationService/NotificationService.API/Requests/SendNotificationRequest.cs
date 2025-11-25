using MediatR;
using NotificationService.Entity.Domain;

namespace NotificationService.API.Requests
{
    public class SendNotificationRequest: IRequest<Notification>
    { 
        public NotificationType NotificationType { get; set; }
        public string Message { get; set; } = string.Empty;
        public string CustomerEmail { get; set; }
    }
}