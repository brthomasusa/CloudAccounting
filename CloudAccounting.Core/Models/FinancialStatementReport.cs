namespace CloudAccounting.Core.Models;

public partial class FinancialStatementReport
{
    public string? ReportCode { get; set; }

    public string? ReportTitle { get; set; }

    public decimal? Srno { get; set; }

    public string? FinancialStatementAccount { get; set; }

    public decimal? CurrentBalance { get; set; }

    public decimal? PreviousBalance { get; set; }

    public decimal? Percent { get; set; }

    public string? UserId { get; set; }

    public string? CompanyName { get; set; }

    public Int16? CompanyYear { get; set; }

    public string? CompanyMonthName { get; set; }

    public bool? Calculation { get; set; }

    public bool? NetValue { get; set; }

    public bool? Notes { get; set; }

    public string? NotesCode { get; set; }

    public string? NotesTitle { get; set; }

    public bool? Heading { get; set; }
}
