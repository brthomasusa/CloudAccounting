using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetCompanyLookupItems
{
    public class GetCompanyLookupItemsQueryHandler
    (
        ILookupRepository repository,
        ILogger<GetCompanyLookupItemsQueryHandler> logger
    ) : IQueryHandler<GetCompanyLookupItemsQuery, List<CompanyLookupItem>>
    {
        private readonly ILookupRepository _repository = repository;
        private readonly ILogger<GetCompanyLookupItemsQueryHandler> _logger = logger;

        public async Task<Result<List<CompanyLookupItem>>> Handle
        (
            GetCompanyLookupItemsQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<List<CompanyLookupItem>> result = await _repository.RetrieveAllAsync();

                if (result.IsSuccess)
                {
                    return result.Value;
                }

                return Result<List<CompanyLookupItem>>.Failure<List<CompanyLookupItem>>(
                    new Error("GetCompanyLookupItemsQueryHandler.Handle", result.Error.Message)
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<CompanyLookupItem>>.Failure<List<CompanyLookupItem>>(
                    new Error("GetCompanyLookupItemsQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}