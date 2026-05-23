namespace CloudAccounting.Core.Models;

public class ChartOfAccounts
{
    public int CompanyCode { get; set; }

    public string AccountCode { get; set; } = null!;

    public string? AccountTitle { get; set; }

    public int? AccountLevel { get; set; }

    public string? AccountClassification { get; set; } // Null, Asset, Liability, Equity, Revenue, Expense

    public string? AccountType { get; set; } // Null, Other, or Bank

    public string? CostCenterCode { get; set; }

    public virtual ICollection<BankOpeningStatement> BankOpeningStatements { get; set; } = [];

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
}
