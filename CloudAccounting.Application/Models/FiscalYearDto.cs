namespace CloudAccounting.Application.Models
{
    public record FiscalYearDto
    (
        int FiscalYear,
        DateTime FiscalYearStartDate,
        DateTime FiscalYearEndDate,
        bool IsInitialFiscalYear,
        bool IsFiscalYearClosed,
        bool HasTransactions,
        DateTime TemporaryYearEndProcessLastExecuted,
        List<FiscalPeriodDto> FiscalPeriods
    );

}