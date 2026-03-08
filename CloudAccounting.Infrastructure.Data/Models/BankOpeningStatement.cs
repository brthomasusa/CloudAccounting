namespace CloudAccounting.Infrastructure.Data.Models;

public partial class BankOpeningStatementDM
{
    public int SrNo { get; set; }

    public int CompanyCode { get; set; }

    public string AccountCode { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public bool Reconciled { get; set; }

    public virtual CompanyDM CompanyCodeNavigation { get; set; } = null!;

    public virtual ChartOfAccountsDM COANavigation { get; set; } = null!;
}
