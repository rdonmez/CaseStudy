using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entity.Domain;

namespace NotificationService.Entity.Repositories
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n=> n.Id == id);    
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications.OrderByDescending(t=>t.CreatedAt).ToListAsync();
        }

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public void UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }

        public void DeleteNotification(Notification notification)
        {
            _context.Notifications.Remove(notification);
            _context.SaveChanges();
        }
    }
}