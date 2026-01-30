namespace CloudAccounting.Infrastructure.Data.Models;

public partial class FinancialStatementSetup
{
    public int CompanyCode { get; set; }

    public string ReportCode { get; set; } = null!;

    public string? ReportTitle { get; set; }

    public string FinancialStatementAccount { get; set; } = null!;

    public string? AccountFrom { get; set; }

    public string? AccountTo { get; set; }
}
