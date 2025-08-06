namespace AuthService.Application.Models.Responses.User
{
    public class GetUserProfileResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
