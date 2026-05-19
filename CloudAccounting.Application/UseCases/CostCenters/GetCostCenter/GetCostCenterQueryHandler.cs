namespace CloudAccounting.Application.UseCases.CostCenters.GetCostCenter;

public class GetCostCenterQueryHandler
(
    ICostCenterRepository repository, 
    ILogger<GetCostCenterQueryHandler> logger
) : IQueryHandler<GetCostCenterQuery, CostCenterDto>
{
    public async Task<Result<CostCenterDto>> Handle
    (
        GetCostCenterQuery query, 
        CancellationToken cancellationToken
    )
    {
        try
        {
            var getCostCenterResult = await repository.RetrieveAsync(query.CompanyCode, query.CostCenterCode);

            if (!getCostCenterResult.IsSuccess)
                return Result.Failure<CostCenterDto>(
                    new Error("GetCostCenterQueryHandler.Handle", getCostCenterResult.Error.Message)
                );
            
            var costCenterDto = getCostCenterResult.Value.Adapt<CostCenterDto>();

            return Result.Success(costCenterDto);
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<CostCenterDto>(
                new Error("GetCostCenterQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }    
}