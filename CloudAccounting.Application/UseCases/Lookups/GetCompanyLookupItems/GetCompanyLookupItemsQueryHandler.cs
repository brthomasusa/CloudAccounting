using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetCompanyLookupItems
{
    public class GetCompanyLookupItemsQueryHandler(
        ILookupRepository repository,
        ILogger<GetCompanyLookupItemsQueryHandler> logger
    ) : IQueryHandler<GetCompanyLookupItemsQuery, List<CompanyLookupItem>>
    {
        public async Task<Result<List<CompanyLookupItem>>> Handle
        (
            GetCompanyLookupItemsQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<List<CompanyLookupItem>> result = await repository.RetrieveAllAsync();

                if (result.IsSuccess)
                {
                    return Result.Success(result.Value);
                }

                return Result.Failure<List<CompanyLookupItem>>(
                    new Error("GetCompanyLookupItemsQueryHandler.Handle", result.Error.Message)
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<CompanyLookupItem>>(
                    new Error("GetCompanyLookupItemsQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}