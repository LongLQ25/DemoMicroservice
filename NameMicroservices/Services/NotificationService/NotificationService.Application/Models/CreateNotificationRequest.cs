namespace NotificationService.Application.Models
{
    public class CreateNotificationRequest
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
    }
}
