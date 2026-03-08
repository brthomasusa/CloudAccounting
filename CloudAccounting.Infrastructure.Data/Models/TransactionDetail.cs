namespace CloudAccounting.Infrastructure.Data.Models;

public partial class TransactionDetailDM
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

    public virtual CompanyDM CompanyCodeNavigation { get; set; } = null!;

    public virtual ChartOfAccountsDM COANavigation { get; set; } = null!;

    public virtual CostCenterDM? CostCenterNavigation { get; set; }

    public virtual TransactionMasterDM TransactionMasterNavigation { get; set; } = null!;
}
