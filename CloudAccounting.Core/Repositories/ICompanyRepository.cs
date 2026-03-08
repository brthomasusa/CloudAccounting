namespace CloudAccounting.Core.Repositories;

public interface ICompanyRepository
{
    Task<Result<Company>> CreateAsync(Company c);

    Task<Result<List<Company>>> RetrieveAllAsync(int pageNumber, int pageSize);

    Task<Result<Company>> RetrieveAsync(int id, CancellationToken token);

    Task<Result<Company>> UpdateAsync(Company c);

    Task<Result> DeleteAsync(int id);

    Task<Result<bool>> IsUniqueCompanyNameForCreate(string companyName);

    Task<Result<bool>> IsUniqueCompanyNameForUpdate(int companyCode, string companyName);

    Task<Result<bool>> IsExistingCompany(int companyCode);

    Task<Result<bool>> CanCompanyBeDeleted(int companyCode);    
}