namespace CloudAccounting.Shared.FiscalYear;

public record FiscalPeriodDto
(
    int MonthId,
    string MonthName,
    DateTime StartDate,
    DateTime EndDate,
    bool PeriodClosed
);
