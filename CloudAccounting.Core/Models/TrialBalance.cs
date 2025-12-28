namespace CloudAccounting.Core.Models;

public partial class TrialBalance
{
    public string? AccountCode { get; set; }

    public string? AccountTitle { get; set; }

    public bool? AccountLevel { get; set; }

    public decimal? OpeningDebit { get; set; }

    public decimal? OpeningCredit { get; set; }

    public decimal? ActivityDebit { get; set; }

    public decimal? ActivityCredit { get; set; }

    public decimal? ClosingDebit { get; set; }

    public decimal? ClosingCredit { get; set; }

    public string? CompanyName { get; set; }

    public DateTime? Tbdate { get; set; }

    public string? FromAccount { get; set; }

    public string? ToAccount { get; set; }

    public string? CostCenterCode { get; set; }

    public string? CostCenterTitle { get; set; }

    public bool? ReportLevel { get; set; }

    public string? UserId { get; set; }

    public bool? GrandTotal { get; set; }
}
