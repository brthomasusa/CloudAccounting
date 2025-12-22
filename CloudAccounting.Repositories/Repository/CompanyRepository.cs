using Microsoft.EntityFrameworkCore.ChangeTracking;     // To use EntityEntry<T>.
using Microsoft.EntityFrameworkCore;                    // To use ToArrayAsync.
using Microsoft.Extensions.Caching.Memory;
using CloudAccounting.EntityModels.Entities;
using CloudAccounting.Repositories.Interface;
using CloudAccounting.DataContext;

namespace CloudAccounting.Repositories.Repository
{
    public class CompanyRepository(CloudAccountingContext ctx, IMemoryCache memoryCache)
        : ICompanyRepository
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

        private readonly CloudAccountingContext _db = ctx;

        public async Task<Company?> CreateAsync(Company c)
        {
            EntityEntry<Company> added = await _db.Companies.AddAsync(c);

            int affected = await _db.SaveChangesAsync();

            if (affected == 1)
            {
                _memoryCache.Set($"company-{c.CompanyCode}", c, _cacheEntryOptions);

                return c;
            }

            return null;
        }

        public async Task<Company[]> RetrieveAllAsync()
        {
            return await _db.Companies.ToArrayAsync();
        }

        public async Task<Company?> RetrieveAsync(int id, CancellationToken token, bool noTracking = false)
        {
            Company? company = null;

            string cacheKey = $"company-{id}";

            if (noTracking)
            {
                company = await _db.Companies.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CompanyCode == id, cancellationToken: token);
            }
            else
            {
                company = await _db.Companies
                    .FirstOrDefaultAsync(c => c.CompanyCode == id, cancellationToken: token);
            }

            _memoryCache.Set(cacheKey, company, _cacheEntryOptions);

            return company;
        }

        public async Task<Company?> UpdateAsync(Company c)
        {
            await _db.Companies.Where(comp => comp.CompanyCode == c.CompanyCode)
                               .ExecuteUpdateAsync(setters => setters
                                    .SetProperty(comp => comp.CompanyName, c.CompanyName)
                                    .SetProperty(comp => comp.Address, c.Address)
                                    .SetProperty(comp => comp.City, c.City)
                                    .SetProperty(comp => comp.Zipcode, c.Zipcode)
                                    .SetProperty(comp => comp.Phone, c.Phone)
                                    .SetProperty(comp => comp.Fax, c.Fax)
                                    .SetProperty(comp => comp.Currency, c.Currency));

            _memoryCache.Set($"company-{c.CompanyCode}", c, _cacheEntryOptions);

            return await _db.Companies.FindAsync(c.CompanyCode);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _db.Companies.Where(c => c.CompanyCode == id)
                               .ExecuteDeleteAsync();

            _memoryCache.Remove($"company-{id}");

            return true;
        }
    }
}