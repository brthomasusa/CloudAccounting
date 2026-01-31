using CloudAccounting.Core.Repositories;

namespace CloudAccounting.Core.Services
{
    public interface ICompanyService
    {
        Task<Result<FiscalYear>> AddFiscalYear(Company company, int fiscalYearNumber, int startMonthNumber);
        void CreateFiscalPeriods(FiscalYear fiscalYear);
    }
}