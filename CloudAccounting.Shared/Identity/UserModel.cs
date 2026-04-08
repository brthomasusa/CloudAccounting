

namespace CloudAccounting.Shared.Identity
{
    public class UserModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public List<UserRoleModel> UserRoles { get; set; } = [];
    }
}