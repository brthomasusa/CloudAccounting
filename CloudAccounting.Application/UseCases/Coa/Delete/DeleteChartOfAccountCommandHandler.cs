
namespace CloudAccounting.Application.UseCases.Coa.Delete;

public class DeleteChartOfAccountCommandHandler(
    IChartOfAccountRepository repo,
    ILogger<DeleteChartOfAccountCommandHandler> logger
) : ICommandHandler<DeleteChartOfAccountCommand, MediatR.Unit>
{
    public async Task<Result<MediatR.Unit>> Handle
    (
        DeleteChartOfAccountCommand command,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await repo.DeleteAsync(command.CompanyCode, command.AccountCode);

            if (!result.IsFailure)
                return Result.Success(MediatR.Unit.Value);

            var errMsg = result.Error.Message;
            logger.LogError("{Message}", errMsg);

            return Result.Failure<MediatR.Unit>(new Error("DeleteChartOfAccountCommandHandler.Handle", errMsg));
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<MediatR.Unit>(new Error("DeleteChartOfAccountCommandHandler.Handle", errMsg));
        }
    }
}