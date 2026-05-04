
using JetBrains.Annotations;

namespace CloudAccounting.Shared.Identity
{
    public class UserModel
    {
        [UsedImplicitly]
        public string? UserId { get; set; } = string.Empty;
        public int CompanyCode { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        [UsedImplicitly]
        public Int16 CompanyYear { get; set; }
        [UsedImplicitly]
        public byte CompanyMonthId { get; set; }
        [UsedImplicitly]
        public string? CompanyMonthName { get; set; } = string.Empty;
        [UsedImplicitly]
        public Int16 GroupId { get; set; }
        [UsedImplicitly]
        public string? Admin { get; set; } = string.Empty;

        [UsedImplicitly]
        public string? GroupTitle { get; set; } = string.Empty;
    }
}