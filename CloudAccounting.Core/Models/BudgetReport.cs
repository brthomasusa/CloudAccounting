namespace CloudAccounting.Core.Models;

public partial class BudgetReport
{
    public string? AccountCode { get; set; }

    public string? AccountTitle { get; set; }

    public decimal? Budget { get; set; }

    public decimal? Actual { get; set; }

    public decimal? Variance { get; set; }

    public decimal? Percent { get; set; }

    public string? Status { get; set; }

    public string? UserId { get; set; }

    public bool? GrandTotal { get; set; }

    public string? CompanyName { get; set; }

    public string? AccountFrom { get; set; }

    public string? AccountTo { get; set; }

    public string? MonthFrom { get; set; }

    public string? MonthTo { get; set; }

    public DateTime? PrintedOn { get; set; }
}
