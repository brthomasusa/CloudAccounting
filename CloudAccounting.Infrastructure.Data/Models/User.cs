namespace CloudAccounting.Infrastructure.Data.Models;

public partial class UserDM
{
    public string UserId { get; set; } = null!;

    public int? CompanyCode { get; set; }

    public Int16? CompanyYear { get; set; }

    public byte? CompanyMonthId { get; set; }

    public Int16? GroupId { get; set; }

    public string? Password { get; set; }

    public string? Admin { get; set; }

    public virtual CompanyDM? CompanyCodeNavigation { get; set; }

    public virtual GroupsMasterDM? GroupsMasterNavigation { get; set; }
}
