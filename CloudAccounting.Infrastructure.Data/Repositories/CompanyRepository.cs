using System.Data;
using CloudAccounting.Core.Exceptions;
using CompanyDataModel = CloudAccounting.Infrastructure.Data.Models.Company;
using CompanyDomainModel = CloudAccounting.Core.Models.Company;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class CompanyRepository
    (
        CloudAccountingContext ctx,
        IMemoryCache memoryCache,
        ILogger<CompanyRepository> logger,
        DapperContext oracleContext,
        IMapper mapper
    )
        : ICompanyRepository
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

        private readonly CloudAccountingContext _db = ctx;
        private readonly ILogger<CompanyRepository> _logger = logger;
        private readonly DapperContext _oracleContext = oracleContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CompanyDomainModel>> CreateAsync(CompanyDomainModel c)
        {
            try
            {
                // Map company domain model to company data model
                CompanyDataModel companyDataModel = _mapper.Map<CompanyDataModel>(c);

                EntityEntry<CompanyDataModel> added = await _db.Companies.AddAsync(companyDataModel);

                int affected = await _db.SaveChangesAsync();

                if (affected == 1)
                {
                    _memoryCache.Set($"company-{c.CompanyCode}", companyDataModel, _cacheEntryOptions);

                    return _mapper.Map<CompanyDomainModel>(companyDataModel);
                }

                return Result<CompanyDomainModel>.Failure<CompanyDomainModel>(
                    new Error("CompanyRepository.CreateAsync", "Create company operation failed!")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyDomainModel>.Failure<CompanyDomainModel>(
                    new Error("CompanyRepository.CreateAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<CompanyDomainModel>>> RetrieveAllAsync(int pageNumber, int pageSize)
        {
            try
            {
                Result<List<CompanyDataModel>> paginatedResults =
                    await _db.Companies
                             .OrderBy(c => c.CompanyName) // Primary sort
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();

                if (paginatedResults.IsSuccess)
                {
                    // List<CompanyDomainModel> companyDomainList = paginatedResults.Value.Adapt<List<CompanyDomainModel>>();
                    return paginatedResults.Value.Adapt<List<CompanyDomainModel>>();
                }

                _logger.LogWarning("There was a problem retrieving a list of companies: {Message}", paginatedResults.Error.Message);

                return Result<List<CompanyDomainModel>>.Failure<List<CompanyDomainModel>>(
                    new Error("CompanyRepository.CreateAsync", paginatedResults.Error.Message)
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<CompanyDomainModel>>.Failure<List<CompanyDomainModel>>(
                    new Error("CompanyRepository.CreateAsync", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }

        public async Task<Result<CompanyDomainModel>> RetrieveAsync(int id, CancellationToken token, bool noTracking = false)
        {
            try
            {
                CompanyDataModel? company = null;

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

                if (company is null)
                {
                    CompanyNotFoundException notFoundException = new(id);
                    return Result<CompanyDomainModel>.Failure<CompanyDomainModel>(
                        new Error("CompanyRepository.RetrieveAsync", notFoundException.Message));
                }

                _memoryCache.Set(cacheKey, company, _cacheEntryOptions);

                return _mapper.Map<CompanyDomainModel>(company);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyDomainModel>.Failure<CompanyDomainModel>(
                    new Error("CompanyRepository.RetrieveAsync", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }

        public async Task<Result<CompanyDomainModel>> UpdateAsync(CompanyDomainModel c)
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

                CompanyDataModel? dataModel = await _db.Companies.FindAsync(c.CompanyCode);

                _memoryCache.Set($"company-{c.CompanyCode}", dataModel, _cacheEntryOptions);

                return _mapper.Map<CompanyDomainModel>(dataModel!);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyDomainModel>.Failure<CompanyDomainModel>(
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

        public async Task<Result> DeleteFiscalYearAsync(int companyCode, int fiscalYear)
        {
            try
            {
                await _db.FiscalYears.Where(c => c.CompanyCode == companyCode && c.CompanyYear == fiscalYear)
                                     .ExecuteDeleteAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure(new Error("CompanyRepository.DeleteFiscalYearAsync", errMsg));
            }
        }
    }
}