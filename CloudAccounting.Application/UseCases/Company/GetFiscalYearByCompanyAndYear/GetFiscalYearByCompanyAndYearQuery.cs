using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Company.GetFiscalYearByCompanyAndYear
{
    public record GetFiscalYearByCompanyAndYearQuery(int CompanyCode, int FiscalYear)
        : IQuery<CompanyWithFiscalPeriodsDto>;
}