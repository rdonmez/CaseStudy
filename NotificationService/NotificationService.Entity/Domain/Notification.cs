#nullable enable
using System;

namespace NotificationService.Entity.Domain
{
    public class Notification
    {
        public int Id { get; set; }
       
        public string Message { get; set; } = string.Empty;
        
        public NotificationStatus Status { get; set; } = NotificationStatus.Waiting;
        public NotificationType Type { get; set; } = NotificationType.Email;
          
        public string? CustomerEmail { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string? ErrorMessage { get; set; } = null;
    }
}