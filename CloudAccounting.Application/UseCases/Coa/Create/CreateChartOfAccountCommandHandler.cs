using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.Create;

public class CreateChartOfAccountCommandHandler(
    IChartOfAccountRepository repo,
    ILogger<CreateChartOfAccountCommandHandler> logger,
    IMapper mapper
) : ICommandHandler<CreateChartOfAccountCommand, ChartOfAccountDto>
{
    public async Task<Result<ChartOfAccountDto>> Handle(CreateChartOfAccountCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var coa = mapper.Map<ChartOfAccounts>(command);

            var result = await repo.CreateAsync(coa);

            if (!result.IsFailure)
                return result.Value.Adapt<ChartOfAccountDto>();

            var errMsg = result.Error.Message;
            logger.LogError("{Message}", errMsg);

            return Result.Failure<ChartOfAccountDto>(new Error("CreateChartOfAccountCommandHandler.Handle", errMsg));
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<ChartOfAccountDto>(new Error("CreateChartOfAccountCommandHandler.Handle", errMsg));
        }
    }
}