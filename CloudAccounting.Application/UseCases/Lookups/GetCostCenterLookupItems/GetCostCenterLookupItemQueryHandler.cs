using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetCostCenterLookupItems;

public class GetCostCenterLookupItemQueryHandler(
    ILookupRepository lookupRepository,
    ILogger<GetCostCenterLookupItemQueryHandler> logger
) : IQueryHandler<GetCostCenterLookupItemQuery, List<CostCenterLookupItem>>
{
    public async Task<Result<List<CostCenterLookupItem>>> Handle
    (
        GetCostCenterLookupItemQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await lookupRepository.RetrieveCostCentersAsync(query.CompanyCode);

            if (!result.IsSuccess)
                return Result.Failure<List<CostCenterLookupItem>>(
                    new Error("GetCostCenterLookupItemQueryHandler.Handle", result.Error.Message)
                );

            return Result.Success(result.Value);
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<CostCenterLookupItem>>(
                new Error("GetCostCenterLookupItemQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}