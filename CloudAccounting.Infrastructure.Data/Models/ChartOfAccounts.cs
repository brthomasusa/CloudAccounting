namespace CloudAccounting.Infrastructure.Data.Models;

public partial class ChartOfAccounts
{
    public int CompanyCode { get; set; }

    public string AccountCode { get; set; } = null!;

    public string? AccountTitle { get; set; }

    public int? AccountLevel { get; set; }

    public string? AccountClassification { get; set; }

    public string? AccountType { get; set; }

    public string? CostCenterCode { get; set; }

    public virtual Company CompanyCodeNavigation { get; set; } = null!;

    public virtual ICollection<BankOpeningStatement> BankOpeningStatements { get; set; } = [];

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
}
