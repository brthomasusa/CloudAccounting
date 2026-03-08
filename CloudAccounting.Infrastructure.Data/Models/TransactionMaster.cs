namespace CloudAccounting.Infrastructure.Data.Models;

public partial class TransactionMasterDM
{
    public int TransactionNumber { get; set; }

    public int CompanyCode { get; set; }

    public Int16 CompanyYear { get; set; }

    public byte CompanyMonth { get; set; }

    public int VoucherCode { get; set; }

    public int VoucherNumber { get; set; }

    public DateTime VoucherDate { get; set; }

    public string Description { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string Verified { get; set; } = null!;

    public string Posted { get; set; } = null!;

    public bool Closing { get; set; }

    public virtual CompanyDM CompanyCodeNavigation { get; set; } = null!;

    public virtual FiscalYearDM FiscalYearNavigation { get; set; } = null!;

    public virtual ICollection<TransactionDetailDM> TransactionDetails { get; set; } = [];

    public virtual VoucherDM VoucherNavigation { get; set; } = null!;
}
