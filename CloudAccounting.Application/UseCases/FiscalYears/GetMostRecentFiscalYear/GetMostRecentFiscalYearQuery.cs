using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Application.UseCases.FiscalYears.GetMostRecentFiscalYear
{
    public record class GetMostRecentFiscalYearQuery(int CompanyCode) : IQuery<FiscalYearDto>;
}