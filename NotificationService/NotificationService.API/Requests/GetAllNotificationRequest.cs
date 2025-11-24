using System.Collections.Generic;
using MediatR;
using NotificationService.Entity.Domain;

namespace NotificationService.API.Requests
{
    public class GetAllNotificationRequest: IRequest<IEnumerable<Notification>>
    {
        
    }
}