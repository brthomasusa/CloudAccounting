namespace CloudAccounting.Infrastructure.Data.Models;

public partial class Budget
{
    public int CompanyCode { get; set; }

    public Int16? CompanyYear { get; set; }

    public string AccountCode { get; set; } = null!;

    public string AccountClassification { get; set; } = null!;

    public string? CostCenterCode { get; set; }

    public decimal? BudgetAmount1 { get; set; }

    public decimal? BudgetAmount2 { get; set; }

    public decimal? BudgetAmount3 { get; set; }

    public decimal? BudgetAmount4 { get; set; }

    public decimal? BudgetAmount5 { get; set; }

    public decimal? BudgetAmount6 { get; set; }

    public decimal? BudgetAmount7 { get; set; }

    public decimal? BudgetAmount8 { get; set; }

    public decimal? BudgetAmount9 { get; set; }

    public decimal? BudgetAmount10 { get; set; }

    public decimal? BudgetAmount11 { get; set; }

    public decimal? BudgetAmount12 { get; set; }

    public bool? Criterion { get; set; }

    public virtual Company CompanyCodeNavigation { get; set; } = null!;

    public virtual ChartOfAccounts COANavigation { get; set; } = null!;
}
