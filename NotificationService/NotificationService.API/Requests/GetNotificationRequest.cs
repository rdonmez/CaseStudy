using System;
using MediatR;
using NotificationService.Entity.Domain;

namespace NotificationService.API.Requests
{
    public class GetNotificationRequest: IRequest<Notification>
    {
        public int Id { get; set; }
    }
}