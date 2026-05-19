namespace CloudAccounting.Application.UseCases.CostCenters.UpdateCostCenter;

public class UpdateCostCenterCommandHandler(
    ICostCenterRepository repo,
    ILogger<UpdateCostCenterCommandHandler> logger
) : ICommandHandler<UpdateCostCenterCommand, CostCenterDto>
{
    public async Task<Result<CostCenterDto>> Handle(UpdateCostCenterCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var costCenter = command.Adapt<CostCenter>();

            var result = await repo.UpdateAsync(costCenter);

            if (!result.IsFailure)
                return result.Value.Adapt<CostCenterDto>();

            var errMsg = result.Error.Message;
            logger.LogError("{Message}", errMsg);

            return Result.Failure<CostCenterDto>(new Error("UpdateCostCenterCommandHandler.Handle", errMsg));
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);
            return Result.Failure<CostCenterDto>(new Error("UpdateCostCenterCommandHandler.Handle", errMsg));
        }
    }
}