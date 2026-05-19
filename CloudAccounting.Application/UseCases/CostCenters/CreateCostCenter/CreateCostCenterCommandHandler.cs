
namespace CloudAccounting.Application.UseCases.CostCenters.CreateCostCenter;

public class CreateCostCenterCommandHandler(
    ICostCenterRepository repo,
    ILogger<CreateCostCenterCommandHandler> logger
) : ICommandHandler<CreateCostCenterCommand, CostCenterDto>
{
    public async Task<Result<CostCenterDto>> Handle(CreateCostCenterCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var costCenter = command.Adapt<CostCenter>();
            costCenter.CostCenterLevel = costCenter.CostCenterCode.Length == 2 ? (byte)1 : (byte)2;

            var result = await repo.CreateAsync(costCenter);

            if (!result.IsFailure)
                return result.Value.Adapt<CostCenterDto>();

            var errMsg = result.Error.Message;
            logger.LogError("{Message}", errMsg);

            return Result.Failure<CostCenterDto>(new Error("CreateCostCenterCommandHandler.Handle", errMsg));
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);
            return Result.Failure<CostCenterDto>(new Error("CreateCostCenterCommandHandler.Handle", errMsg));
        }
    }
}