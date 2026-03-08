
namespace CloudAccounting.Core.Repositories
{
    public interface IFiscalYearRepository
    {
        Task<Result<FiscalYear>> GetFiscalYearAsync(int companyCode, int fiscalYearNumber);

        Task<Result<FiscalYear>> InsertFiscalYearAsync(FiscalYear fiscalYear);

        Task<Result> DeleteFiscalYearAsync(int companyCode, int fiscalYear);

        Task<Result<bool>> CanFiscalYearBeDeleted(int companyCode, int fiscalYearNumber);

        Task<Result<DateTime>> EarliestNextFiscalYearStartDate(int companyCode);

        Task<Result<string>> GetCompanyName(int companyCode);

        Task<Result<bool>> DoesCompanyHaveInitialFiscalYear(int companyCode);

        Task<Result<bool>> IsValidCompanyCode(int companyCode);

        Task<Result<bool>> IsUniqueFiscalYearNumber(int companyCode, int fiscalYearNumber);
    }
}