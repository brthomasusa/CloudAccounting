namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public interface ILookupRepository
    {
        Task<Result<List<CompanyLookupItem>>> RetrieveAllAsync();

        Task<Result<List<CostCenterLookupItem>>> RetrieveCostCentersAsync(int companyCode);
    }
}