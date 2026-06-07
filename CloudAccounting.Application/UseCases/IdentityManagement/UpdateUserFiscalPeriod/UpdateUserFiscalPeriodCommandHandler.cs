namespace CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserFiscalPeriod;

public class UpdateUserFiscalPeriodCommandHandler(
    IUserRepository repo,
    ILogger<UpdateUserFiscalPeriodCommandHandler> logger
) : ICommandHandler<UpdateUserFiscalPeriodCommand, MediatR.Unit>
{
    public async Task<Result<MediatR.Unit>> Handle(UpdateUserFiscalPeriodCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await repo.UpdateAllUsersFiscalPeriodAsync(command.CompanyCode, command.CompanyYear,
                    command.CompanyMonthId);

            if (!result.IsFailure)
                return MediatR.Unit.Value;

            var errMsg = result.Error.Message;
            logger.LogError("{Message}", errMsg);

            return Result.Failure<MediatR.Unit>(new Error("UpdateUserFiscalPeriodCommandHandler.Handle", errMsg));
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);
            return Result.Failure<MediatR.Unit>(new Error("UpdateUserFiscalPeriodCommandHandler.Handle", errMsg));
        }
    }
}