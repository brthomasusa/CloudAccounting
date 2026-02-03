using CloudAccounting.Shared.Company;

namespace CloudAccounting.Infrastructure.Data.Interfaces
{
    public interface ICompanyReadRepository
    {
        Task<Result<CompanyDto>> GetCompanyDtoByIdAsync(int companyCode);
        Task<Result<CompanyWithFiscalPeriodsDto>> GetFiscalYearDtoByIdAndYearAsync(int companyCode, int fiscalYearNumber);
        Task<Result<bool>> IsUniqueCompanyNameForCreate(string companyName);

        Task<Result<bool>> IsUniqueCompanyNameForUpdate(int companyCode, string companyName);

        Task<Result<bool>> IsExistingCompany(int companyCode);

        Task<Result<bool>> CanCompanyBeDeleted(int companyCode);

        Task<Result<bool>> CanCompanyFiscalYearBeDeleted(int companyCode, int yearNumber);

        Task<Result<bool>> InitialFiscalYearExist(int companyCode);

        Task<Result<string>> GetCompanyName(int companyCode);
    }
}