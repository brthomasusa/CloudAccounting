namespace CloudAccounting.Shared.Company
{
    public record class FiscalYearDto
    (
        int CompanyCode,
        string CompanyName,
        int Year,
        DateTime FiscalYearStartDate,
        DateTime FiscalYearEndDate,
        bool IsInitialFiscalYear,
        bool IsFiscalYearClosed,
        bool HasTransactions,
        DateTime? TemporaryYearEndProcessLastExecuted,
        List<FiscalPeriodDto> FiscalPeriods
    );
}