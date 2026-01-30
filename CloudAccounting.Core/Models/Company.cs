namespace CloudAccounting.Core.Models;

public partial class Company
{
    public int CompanyCode { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? City { get; set; }

    public string? Zipcode { get; set; }

    public string? Currency { get; set; }

    public virtual ICollection<ChartOfAccounts> ChartOfAccounts { get; set; } = [];

    public virtual ICollection<FiscalYear> FiscalYears { get; set; } = [];
}
