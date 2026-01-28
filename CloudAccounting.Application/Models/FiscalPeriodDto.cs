namespace CloudAccounting.Application.Models
{
    public record FiscalPeriodDto
    (
        int MonthId,
        string MonthName,
        DateTime StartDate,
        DateTime EndDate,
        bool PeriodClosed
    );
}