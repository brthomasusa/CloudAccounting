namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public interface ILookupRepository
    {
        Task<Result<List<CompanyLookupItem>>> RetrieveAllAsync();
    }
}