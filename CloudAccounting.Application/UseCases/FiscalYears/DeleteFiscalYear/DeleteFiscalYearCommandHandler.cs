namespace CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;

public class DeleteFiscalYearCommandHandler
(
    IFiscalYearRepository repository,
    ILogger<DeleteFiscalYearCommandHandler> logger
) : ICommandHandler<DeleteFiscalYearCommand, MediatR.Unit>
{
    public async Task<Result<MediatR.Unit>> Handle(DeleteFiscalYearCommand command, CancellationToken token)
    {
        Result result = await repository.DeleteFiscalYearAsync(command.CompanyCode, command.FiscalYear);

        if (result.IsFailure)
        {
            string errorMessage = result.Error.Message;
            logger.LogWarning("There was a problem deleting the fiscal year: {ERROR}", errorMessage);

            return Result.Failure<MediatR.Unit>(
                new Error("DeleteFiscalYearCommandHandler.Handle", errorMessage)
            );
        }

        return MediatR.Unit.Value;
    }
}
