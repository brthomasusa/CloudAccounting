namespace CloudAccounting.Infrastructure.Data.Models;

public partial class CostCenterDM
{
    public int CompanyCode { get; set; }

    public string CostCenterCode { get; set; } = null!;

    public string? CostCenterTitle { get; set; }

    public byte? CostCenterLevel { get; set; }

    public virtual CompanyDM CompanyCodeNavigation { get; set; } = null!;

    public virtual ICollection<TransactionDetailDM> TransactionDetails { get; set; } = [];
}
