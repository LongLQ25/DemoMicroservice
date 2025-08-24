using NotificationService.Domain.Entities;

namespace NotificationService.Application.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification, Guid>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId);
    }
}
