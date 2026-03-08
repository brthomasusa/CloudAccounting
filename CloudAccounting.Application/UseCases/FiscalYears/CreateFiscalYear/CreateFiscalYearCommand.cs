using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear
{
    public record class CreateFiscalYearCommand
    (
        int CompanyCode,
        int FiscalYearNumber,
        DateTime StartDate
    ) : ICommand<FiscalYearDto>;
}