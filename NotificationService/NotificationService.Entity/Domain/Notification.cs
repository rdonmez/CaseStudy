#nullable enable
using System;

namespace NotificationService.Entity.Domain
{
    public class Notification
    {
        public int Id { get; set; }
        
        public string Message { get; set; } = string.Empty;
        
        public NotificationType NotificationType { get; set; }
        
        public NotificationStatus Status { get; set; } = NotificationStatus.Waiting;
        
        public int CustomerId { get; set; }
        
        public string CustomerEmail { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public string? ErrorMessage { get; set; } = null;
    }
}