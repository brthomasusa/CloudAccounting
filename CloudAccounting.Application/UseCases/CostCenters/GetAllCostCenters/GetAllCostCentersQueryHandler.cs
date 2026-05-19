

namespace CloudAccounting.Application.UseCases.CostCenters.GetAllCostCenters;

public class GetAllCostCentersQueryHandler
(
    ICostCenterRepository repository, 
    ILogger<GetAllCostCentersQueryHandler> logger
) : IQueryHandler<GetAllCostCentersQuery, List<CostCenterDto>>
{
    public async Task<Result<List<CostCenterDto>>> Handle
    (
        GetAllCostCentersQuery query, 
        CancellationToken cancellationToken
    )
    {
        try
        {
            var getAllCostCentersResult = await repository.RetrieveAllAsync(query.CompanyCode);

            if (!getAllCostCentersResult.IsSuccess)
                return Result.Failure<List<CostCenterDto>>(
                    new Error("GetAllCostCentersQueryHandler.Handle", getAllCostCentersResult.Error.Message)
                );
            
            var costCenterDtos = getAllCostCentersResult.Value.Adapt<List<CostCenterDto>>();

            return Result.Success(costCenterDtos);
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<CostCenterDto>>(
                new Error("GetAllCostCentersQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }   
}