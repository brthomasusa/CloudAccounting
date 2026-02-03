using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Company.CreateFiscalYear
{
    public record class CreateFiscalYearCommand
    (
        int CompanyCode,
        int FiscalYear,
        int StartMonthNumber
    ) : ICommand<CompanyWithFiscalPeriodsDto>;
}