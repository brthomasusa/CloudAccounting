using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetCoaLookupItems;

public class GetCoaLookupItemsQueryHandler(
    ILookupRepository lookupRepository,
    ILogger<GetCoaLookupItemsQueryHandler> logger
) : IQueryHandler<GetCoaLookupItemsQuery, List<CoaLookupItem>>
{
    public async Task<Result<List<CoaLookupItem>>> Handle
    (
        GetCoaLookupItemsQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await lookupRepository.RetrieveLedgerAccountsAsync(query.CompanyCode);

            if (!result.IsSuccess)
                return Result.Failure<List<CoaLookupItem>>(
                    new Error("GetCoaLookupItemsQueryHandler.Handle", result.Error.Message)
                );

            return Result.Success(result.Value);
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<CoaLookupItem>>(
                new Error("GetCoaLookupItemsQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}