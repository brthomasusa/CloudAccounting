using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Company.GetCompanyWithNoFiscalYearData
{
    public record GetCompanyWithNoFiscalYearDataQuery(int CompanyCode)
        : IQuery<CompanyWithFiscalPeriodsDto>;
}