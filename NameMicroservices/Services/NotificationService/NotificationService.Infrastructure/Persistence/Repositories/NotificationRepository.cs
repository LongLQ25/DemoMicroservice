using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Repositories;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence.DBContext;

namespace NotificationService.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository(AppDBContext context) : GenericRepository<Notification, Guid>(context), INotificationRepository
    {
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId && x.Channel == "Push" && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead && x.Channel == "Push" && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
