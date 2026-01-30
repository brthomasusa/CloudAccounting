namespace CloudAccounting.Core.Models;

public partial class TransactionMaster
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

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
}
