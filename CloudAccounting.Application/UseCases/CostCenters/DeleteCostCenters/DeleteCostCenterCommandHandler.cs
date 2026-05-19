namespace CloudAccounting.Application.UseCases.CostCenters.DeleteCostCenters;

public class DeleteCostCenterCommandHandler(
    ICostCenterRepository repo,
    ILogger<DeleteCostCenterCommandHandler> logger
) : ICommandHandler<DeleteCostCenterCommand, MediatR.Unit>
{
    public async Task<Result<MediatR.Unit>> Handle
    (
        DeleteCostCenterCommand command,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await repo.DeleteAsync(command.CompanyCode, command.CostCenterCode);

            if (!result.IsFailure)
                return MediatR.Unit.Value;

            var errMsg = result.Error.Message;
            logger.LogError("{Message}", errMsg);

            return Result.Failure<MediatR.Unit>(new Error("DeleteCostCenterCommandHandler.Handle", errMsg));
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);
            return Result.Failure<MediatR.Unit>(new Error("DeleteCostCenterCommandHandler.Handle", errMsg));
        }
    }
}