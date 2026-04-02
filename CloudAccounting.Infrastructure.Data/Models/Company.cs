namespace CloudAccounting.Infrastructure.Data.Models;

public partial class CompanyDM
{
    public int CompanyCode { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? City { get; set; }

    public string? Zipcode { get; set; }

    public string? Currency { get; set; }

    public string? DomainName { get; set; }

    public virtual ICollection<BankOpeningStatementDM> BankOpeningStatements { get; set; } = [];

    public virtual ICollection<ChartOfAccountsDM> ChartOfAccounts { get; set; } = [];

    public virtual ICollection<CostCenterDM> CostCenters { get; set; } = [];

    public virtual ICollection<FiscalYearDM> FiscalYears { get; set; } = [];

    public virtual ICollection<TransactionDetailDM> TransactionDetails { get; set; } = [];

    public virtual ICollection<TransactionMasterDM> TransactionMasters { get; set; } = [];

    public virtual ICollection<UserDM> Users { get; set; } = [];
}
