namespace CloudAccounting.Core.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public int? CompanyCode { get; set; }

    public Int16? CompanyYear { get; set; }

    public byte? CompanyMonthId { get; set; }

    public Int16? GroupId { get; set; }

    public string? Password { get; set; }

    public string? Admin { get; set; }

    public virtual Company? CompanyCodeNavigation { get; set; }

    public virtual GroupsMaster? GroupsMasterNavigation { get; set; }
}
