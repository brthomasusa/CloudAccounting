using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Application.UseCases.FiscalYears.GetFiscalYear;

public record class GetFiscalYearQuery(int CompanyCode, int FiscalYearNumber) : IQuery<FiscalYearDto>;

