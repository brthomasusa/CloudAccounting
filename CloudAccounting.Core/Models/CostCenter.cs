namespace CloudAccounting.Core.Models;

public partial class CostCenter
{
    public int CompanyCode { get; set; }

    public string CostCenterCode { get; set; } = null!;

    public string? CostCenterTitle { get; set; }

    public bool? CostCenterLevel { get; set; }

    public virtual Company CompanyCodeNavigation { get; set; } = null!;

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
}
