namespace CloudAccounting.EntityModels.Entities;

public partial class Voucher
{
    public int VoucherCode { get; set; }

    public string? VoucherType { get; set; }

    public string? VoucherTitle { get; set; }

    public bool? VoucherNature { get; set; }

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = [];
}
