using NotificationService.Domain.Base;

namespace NotificationService.Domain.Entities
{
    public class Notification : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; } // Email, SMS, Push
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
