using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetVoucherTypeLookupItems;

public class GetVoucherTypeLookupItemsQueryHandler(
    ILookupRepository lookupRepository,
    ILogger<GetVoucherTypeLookupItemsQueryHandler> logger
) : IQueryHandler<GetVoucherTypeLookupItemsQuery, List<VoucherTypeLookupItem>>
{
    public async Task<Result<List<VoucherTypeLookupItem>>> Handle
    (
        GetVoucherTypeLookupItemsQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await lookupRepository.RetrieveVoucherTypesAsync();

            if (!result.IsSuccess)
                return Result.Failure<List<VoucherTypeLookupItem>>(
                    new Error("GetVoucherTypeLookupItemsQueryHandler.Handle", result.Error.Message)
                );

            return Result.Success(result.Value);
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<VoucherTypeLookupItem>>(
                new Error("GetVoucherTypeLookupItemsQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}