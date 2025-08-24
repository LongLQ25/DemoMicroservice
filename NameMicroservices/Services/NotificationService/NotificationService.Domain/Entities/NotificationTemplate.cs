using NotificationService.Domain.Base;

namespace NotificationService.Domain.Entities
{
    public class NotificationTemplate : BaseEntity<Guid>
    {
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } // HTML or text
        public string Channel { get; set; }
    }
}
