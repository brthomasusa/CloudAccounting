namespace CloudAccounting.Core.Models;

public class User
{
    public string UserId { get; set; } = string.Empty;

    public int CompanyCode { get; set; }

    public Int16 CompanyYear { get; set; }

    public byte CompanyMonthId { get; set; }

    public Int16 GroupId { get; set; }

    public string? Admin { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;
}
