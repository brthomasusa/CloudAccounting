namespace CloudAccounting.Core.Models;

public record FiscalYear
(
    int Year,
    DateTime FiscalYearStartDate,
    DateTime FiscalYearEndDate,
    bool IsInitialFiscalYear,
    bool IsFiscalYearClosed,
    bool HasTransactions,
    DateTime TemporaryYearEndProcessLastExecuted,
    List<FiscalPeriod> FiscalPeriods
);
