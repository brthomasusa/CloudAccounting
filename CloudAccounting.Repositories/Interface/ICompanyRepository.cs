using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.Repositories.Interface
{
    public interface ICompanyRepository
    {
        Task<Company?> CreateAsync(Company c);

        Task<Company[]> RetrieveAllAsync();

        Task<Company?> RetrieveAsync(int id, CancellationToken token, bool noTracking = false);

        Task<Company?> UpdateAsync(Company c);

        Task<bool> DeleteAsync(int id);
    }
}