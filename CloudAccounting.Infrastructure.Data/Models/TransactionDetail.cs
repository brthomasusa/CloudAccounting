namespace CloudAccounting.Infrastructure.Data.Models;

public partial class TransactionDetail
{
    public int LineNumber { get; set; }

    public int TransactionNumber { get; set; }

    public int CompanyCode { get; set; }

    public string AccountCode { get; set; } = null!;

    public string? CostCenterCode { get; set; }

    public string Description { get; set; } = null!;

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public string? Reference { get; set; }

    public bool Reconciled { get; set; }

    public virtual Company CompanyCodeNavigation { get; set; } = null!;

    public virtual ChartOfAccounts COANavigation { get; set; } = null!;

    public virtual CostCenter? CostCenterNavigation { get; set; }

    public virtual TransactionMaster TransactionMasterNavigation { get; set; } = null!;
}
