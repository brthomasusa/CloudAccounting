using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class LookupRepository(
        AppDbContext context,
        ILogger<LookupRepository> logger
    ) : ILookupRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<LookupRepository> _logger = logger;

        public async Task<Result<List<CompanyLookupItem>>> RetrieveAllAsync()
        {
            try
            {
                List<CompanyLookupItem> companies = await _context.Companies
                    .Select(c => new CompanyLookupItem
                    {
                        CompanyCode = c.CompanyCode,
                        CompanyName = c.CompanyName
                    })
                    .ToListAsync();

                return companies;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<CompanyLookupItem>>(
                    new Error("LookupRepository.RetrieveAllAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<CostCenterLookupItem>>> RetrieveCostCentersAsync(int companyCode)
        {
            try
            {
                List<CostCenterLookupItem> costCenters = await _context.CostCenters
                    .Where(cc => cc.CompanyCode == companyCode)
                    .Select(cc => new CostCenterLookupItem
                    {
                        CostCenterCode = cc.CostCenterCode,
                        CostCenterTitle = cc.CostCenterTitle
                    })
                    .ToListAsync();

                return costCenters;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<CostCenterLookupItem>>(
                    new Error("LookupRepository.RetrieveCostCentersAsync", errMsg)
                );
            }
        }
    }
}