
namespace CloudAccounting.Core.Repositories
{
    public interface IFiscalYearRepository
    {
        Task<Result<FiscalYear>> GetFiscalYearByCompanyAndYearAsync(int companyCode, int fiscalYearNumber);
        Task<Result<FiscalYear>> InsertFiscalYearAsync(FiscalYear fiscalYear);
    }
}