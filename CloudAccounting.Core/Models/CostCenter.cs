namespace CloudAccounting.Core.Models;

public class CostCenter
{
    public int CompanyCode { get; set; }

    public string CostCenterCode { get; set; } = null!;

    public string CostCenterTitle { get; set; } = null!;

    public byte CostCenterLevel { get; set; }

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
}
