using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class LookupRepository
    (
        CloudAccountingContext context,
        ILogger<LookupRepository> logger
    ) : ILookupRepository
    {
        private readonly CloudAccountingContext _context = context;
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

                return Result<List<CompanyLookupItem>>.Failure<List<CompanyLookupItem>>(
                    new Error("LookupRepository.RetrieveAllAsync", errMsg)
                );
            }
        }
    }
}