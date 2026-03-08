namespace CloudAccounting.Infrastructure.Data.Models;

public partial class VoucherDM
{
    public int VoucherCode { get; set; }

    public string? VoucherType { get; set; }

    public string? VoucherTitle { get; set; }

    public byte? VoucherClassification { get; set; }

    public virtual ICollection<TransactionMasterDM> TransactionMasters { get; set; } = [];
}
