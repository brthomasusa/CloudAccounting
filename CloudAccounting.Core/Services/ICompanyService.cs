using CloudAccounting.Core.Repositories;

namespace CloudAccounting.Core.Services
{
    public interface ICompanyService
    {
        Task<Result<FiscalYear>> CreateFiscalYearWithPeriods(int companyCode, int fiscalYearNumber, int startMonthNumber);

    }
}