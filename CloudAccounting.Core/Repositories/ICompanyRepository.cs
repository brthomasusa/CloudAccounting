namespace CloudAccounting.Core.Repositories;

public interface ICompanyRepository
{
    Task<Result<Company>> CreateAsync(Company c);

    Task<Result<List<Company>>> RetrieveAllAsync(int pageNumber, int pageSize);

    Task<Result<Company>> RetrieveAsync(int id, CancellationToken token, bool noTracking = false);

    Task<Result<Company>> UpdateAsync(Company c);

    Task<Result> DeleteAsync(int id);

    Task<Result> DeleteFiscalYearAsync(int companyCode, int fiscalYear);
}