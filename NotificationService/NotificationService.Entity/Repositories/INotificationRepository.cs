using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.Entity.Domain;

namespace NotificationService.Entity.Repositories
{
    public interface INotificationRepository
    { 
        Task<Notification> GetNotificationByIdAsync(int id);
 
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
 
        Task<Notification> AddNotificationAsync(Notification notification);
 
        void UpdateNotification(Notification notification);
 
        void DeleteNotification(Notification notification);
    }
}