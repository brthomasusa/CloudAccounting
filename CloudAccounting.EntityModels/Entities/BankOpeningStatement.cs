namespace CloudAccounting.EntityModels.Entities;

public partial class BankOpeningStatement
{
    public int SrNo { get; set; }

    public int CompanyCode { get; set; }

    public string AccountCode { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public bool Reconciled { get; set; }

    public virtual Company CompanyCodeNavigation { get; set; } = null!;

    public virtual ChartOfAccounts COANavigation { get; set; } = null!;
}
