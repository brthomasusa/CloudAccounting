

namespace CloudAccounting.Shared.Identity
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int CompanyCode { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public Int16 CompanyYear { get; set; }
        public byte CompanyMonthId { get; set; }
        public string CompanyMonthName { get; set; } = string.Empty;
        public bool IsSystemAdmin { get; set; }
        public bool IsCompanyAdmin { get; set; }
        public UserRoleModel? UserRole { get; set; }
    }
}