namespace AuthService.Application.Models.Requests.User
{
    public class UpdateUserRequest
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
    }
}
