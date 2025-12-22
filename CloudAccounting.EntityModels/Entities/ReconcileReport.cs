namespace CloudAccounting.EntityModels.Entities;

public partial class ReconcileReport
{
    public int? Srno { get; set; }

    public string? UserId { get; set; }

    public string? CompanyName { get; set; }

    public DateTime? ReportDate { get; set; }

    public string? AccountCode { get; set; }

    public string? AccountTitle { get; set; }

    public string? MonthYear { get; set; }

    public DateTime? VoucherDate { get; set; }

    public string? VoucherType { get; set; }

    public int? VoucherNumber { get; set; }

    public string? Description { get; set; }

    public string? Reference { get; set; }

    public decimal? Amount { get; set; }
}
