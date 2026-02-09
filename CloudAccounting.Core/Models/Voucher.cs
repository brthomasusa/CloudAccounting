namespace CloudAccounting.Core.Models;

public partial class Voucher
{
    public int VoucherCode { get; set; }

    public string? VoucherType { get; set; }

    public string? VoucherTitle { get; set; }

    public Int16 VoucherClassification { get; set; }

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = [];
}
