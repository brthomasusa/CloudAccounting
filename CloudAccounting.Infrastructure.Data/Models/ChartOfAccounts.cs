namespace CloudAccounting.Infrastructure.Data.Models;

public partial class ChartOfAccountsDM
{
    public int CompanyCode { get; set; }

    public string AccountCode { get; set; } = null!;

    public string? AccountTitle { get; set; }

    public byte? AccountLevel { get; set; }

    public string? AccountClassification { get; set; }

    public string? AccountType { get; set; }

    public string? CostCenterCode { get; set; }

    public virtual CompanyDM CompanyCodeNavigation { get; set; } = null!;

    public virtual ICollection<BankOpeningStatementDM> BankOpeningStatements { get; set; } = [];

    public virtual ICollection<TransactionDetailDM> TransactionDetails { get; set; } = [];
}
