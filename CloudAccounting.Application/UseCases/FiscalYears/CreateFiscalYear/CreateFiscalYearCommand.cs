using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear
{
    public record class CreateFiscalYearCommand
    (
        int CompanyCode,
        int FiscalYear,
        DateTime StartDate
    ) : ICommand<FiscalYearDto>;
}