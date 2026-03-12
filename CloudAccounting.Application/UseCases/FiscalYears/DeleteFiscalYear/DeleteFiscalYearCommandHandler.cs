namespace CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;

public class DeleteFiscalYearCommandHandler
(
    IFiscalYearRepository repository,
    ILogger<DeleteFiscalYearCommandHandler> logger
) : ICommandHandler<DeleteFiscalYearCommand, MediatR.Unit>
{
    private readonly IFiscalYearRepository _repository = repository;
    private readonly ILogger<DeleteFiscalYearCommandHandler> _logger = logger;

    public async Task<Result<MediatR.Unit>> Handle(DeleteFiscalYearCommand command, CancellationToken token)
    {
        Result result = await _repository.DeleteFiscalYearAsync(command.CompanyCode, command.FiscalYear);

        if (result.IsFailure)
        {
            string errorMessage = result.Error.Message ?? "An unknown error occurred while deleting the fiscal year.";
            _logger.LogWarning("There was a problem deleting the fiscal year: {ERROR}", errorMessage);

            return Result<MediatR.Unit>.Failure<MediatR.Unit>(
                new Error("DeleteFiscalYearCommandHandler.Handle", errorMessage)
            );
        }

        return MediatR.Unit.Value;
    }
}
