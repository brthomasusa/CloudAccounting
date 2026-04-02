using System.Data;
using CloudAccounting.Core.Exceptions;
using CloudAccounting.Infrastructure.Data.Models;
using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Data;


namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class CompanyRepository
    (
        AppDbContext ctx,
        IMemoryCache memoryCache,
        ILogger<CompanyRepository> logger,
        IMapper mapper
    )
        : ICompanyRepository
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

        private readonly AppDbContext _db = ctx;
        private readonly ILogger<CompanyRepository> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<Company>> CreateAsync(Company c)
        {
            try
            {
                // Map company domain model to company data model
                CompanyDM companyDM = _mapper.Map<CompanyDM>(c);

                EntityEntry<CompanyDM> added = await _db.Companies.AddAsync(companyDM);

                int affected = await _db.SaveChangesAsync();

                if (affected == 1)
                {
                    _memoryCache.Set($"company-{c.CompanyCode}", companyDM, _cacheEntryOptions);

                    return _mapper.Map<Company>(companyDM);
                }

                return Result<Company>.Failure<Company>(
                    new Error("CompanyRepository.CreateAsync", "Create company operation failed!")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<Company>.Failure<Company>(
                    new Error("CompanyRepository.CreateAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<Company>>> RetrieveAllAsync(int pageNumber, int pageSize)
        {
            try
            {
                Result<List<CompanyDM>> paginatedResults =
                    await _db.Companies
                             .OrderBy(c => c.CompanyName) // Primary sort
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();

                if (paginatedResults.IsSuccess)
                {
                    // List<Company> companyDomainList = paginatedResults.Value.Adapt<List<Company>>();
                    return paginatedResults.Value.Adapt<List<Company>>();
                }

                _logger.LogWarning("There was a problem retrieving a list of companies: {Message}", paginatedResults.Error.Message);

                return Result<List<Company>>.Failure<List<Company>>(
                    new Error("CompanyRepository.CreateAsync", paginatedResults.Error.Message)
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<Company>>.Failure<List<Company>>(
                    new Error("CompanyRepository.CreateAsync", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }

        public async Task<Result<Company>> RetrieveAsync(int id, CancellationToken token)
        {
            try
            {
                string cacheKey = $"company-{id}";

                CompanyDM? company = await _db.Companies.AsNoTracking()
                        .SingleOrDefaultAsync(c => c.CompanyCode == id, cancellationToken: token);

                if (company is null)
                {
                    CompanyNotFoundException notFoundException = new(id);
                    return Result<Company>.Failure<Company>(
                        new Error("CompanyRepository.RetrieveAsync", notFoundException.Message));
                }

                _memoryCache.Set(cacheKey, company, _cacheEntryOptions);

                return _mapper.Map<Company>(company);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<Company>.Failure<Company>(
                    new Error("CompanyRepository.RetrieveAsync", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }

        public async Task<Result<Company>> UpdateAsync(Company c)
        {
            try
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

                CompanyDM? dataModel = await _db.Companies.FindAsync(c.CompanyCode);

                _memoryCache.Set($"company-{c.CompanyCode}", dataModel, _cacheEntryOptions);

                return _mapper.Map<Company>(dataModel!);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<Company>.Failure<Company>(
                    new Error("CompanyRepository.UpdateAsync", errMsg)
                );
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                await _db.Companies.Where(c => c.CompanyCode == id)
                                   .ExecuteDeleteAsync();

                _memoryCache.Remove($"company-{id}");

                return Result.Success();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure(new Error("CompanyRepository.DeleteAsync", errMsg));
            }
        }

        public async Task<Result<bool>> IsUniqueCompanyNameForCreate(string companyName)
        {
            try
            {
                bool isUnique = await _db.Companies.AllAsync(c => c.CompanyName != companyName);
                return Result<bool>.Success(isUnique);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyRepository.IsUniqueCompanyNameForCreate", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsUniqueCompanyNameForUpdate(int companyCode, string companyName)
        {
            try
            {
                bool isUnique = await _db.Companies
                                         .AnyAsync(c => c.CompanyName == companyName && c.CompanyCode != companyCode);

                return Result<bool>.Success(!isUnique);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyRepository.IsUniqueCompanyNameForUpdate", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsExistingCompany(int companyCode)
        {
            try
            {
                bool exists = await _db.Companies.AnyAsync(c => c.CompanyCode == companyCode);
                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyRepository.IsExistingCompany", errMsg)
                );
            }
        }

        public async Task<Result<bool>> CanCompanyBeDeleted(int companyCode)
        {
            try
            {
                bool hasFiscalYears = await _db.FiscalYears.AnyAsync(c => c.CompanyCode == companyCode);

                return Result<bool>.Success(!hasFiscalYears);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyRepository.CanCompanyBeDeleted", errMsg)
                );
            }
        }
    }
}