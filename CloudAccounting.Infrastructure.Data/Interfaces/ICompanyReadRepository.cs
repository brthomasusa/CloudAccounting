using CloudAccounting.Shared.Company;

namespace CloudAccounting.Infrastructure.Data.Interfaces
{
    public interface ICompanyReadRepository
    {
        Task<Result<CompanyDto>> GetCompanyDtoByIdAsync(int companyCode);
        Task<Result<CompanyWithFiscalPeriodsDto>> GetFiscalYearDtoByIdAndYearAsync(int companyCode, int fiscalYearNumber);
    }
}