using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.Update;

public class UpdateChartOfAccountCommandHandler(
    IChartOfAccountRepository repo,
    ILogger<UpdateChartOfAccountCommandHandler> logger
) : ICommandHandler<UpdateChartOfAccountCommand, ChartOfAccountDto>
{
    public async Task<Result<ChartOfAccountDto>> Handle
    (
        UpdateChartOfAccountCommand command,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var coa = command.Adapt<ChartOfAccounts>();

            var result = await repo.UpdateAsync(coa);

            if (!result.IsFailure)
                return result.Value.Adapt<ChartOfAccountDto>();

            var errMsg = result.Error.Message;
            logger.LogError("{Message}", errMsg);

            return Result.Failure<ChartOfAccountDto>(new Error("UpdateChartOfAccountCommandHandler.Handle", errMsg));
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<ChartOfAccountDto>(new Error("DeleteChartOfAccountCommandHandler.Handle", errMsg));
        }
    }
}