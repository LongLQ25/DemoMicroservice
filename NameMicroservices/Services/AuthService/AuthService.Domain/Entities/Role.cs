using AuthService.Domain.Base;

namespace AuthService.Domain.Entities
{
    public class Role : BaseEntity<Guid>
    {
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
