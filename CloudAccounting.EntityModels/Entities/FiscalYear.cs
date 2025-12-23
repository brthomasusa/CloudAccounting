namespace CloudAccounting.EntityModels.Entities;


public partial class FiscalYear
{
    public int CompanyCode { get; set; }

    public Int16 CompanyYear { get; set; }

    public byte CompanyMonthId { get; set; }

    public string? CompanyMonthName { get; set; }

    public DateTime? PeriodFrom { get; set; }

    public DateTime? PeriodTo { get; set; }

    public bool? InitialYear { get; set; }

    public bool? YearClosed { get; set; }

    public bool? MonthClosed { get; set; }

    public DateTime? TyeExecuted { get; set; }

    public virtual Company CompanyCodeNavigation { get; set; } = null!;

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = [];
}
