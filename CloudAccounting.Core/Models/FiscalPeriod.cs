namespace CloudAccounting.Core.Models
{
    public record FiscalPeriod
    (
        int MonthId,
        string MonthName,
        DateTime StartDate,
        DateTime EndDate,
        bool PeriodClosed
    );
}