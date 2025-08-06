using AuthService.Domain.Base;

namespace AuthService.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
