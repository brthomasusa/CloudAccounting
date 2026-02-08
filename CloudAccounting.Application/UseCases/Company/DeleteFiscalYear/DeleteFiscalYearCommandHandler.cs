namespace CloudAccounting.Application.UseCases.Company.DeleteFiscalYear
{
    public class DeleteFiscalYearCommandHandler
    (
    ICompanyRepository repository,
    ILogger<DeleteFiscalYearCommandHandler> logger
    ) : ICommandHandler<DeleteFiscalYearCommand, MediatR.Unit>
    {
        private readonly ICompanyRepository _repository = repository;
        private readonly ILogger<DeleteFiscalYearCommandHandler> _logger = logger;

        public async Task<Result<MediatR.Unit>> Handle(DeleteFiscalYearCommand command, CancellationToken token)
        {
            Result result = await _repository.DeleteFiscalYearAsync(command.CompanyCode, command.FiscalYear);

            if (result.IsFailure)
            {
                return Result<MediatR.Unit>.Failure<MediatR.Unit>(
                    new Error("DeleteCompanyCommandHandler.Handle", result.Error.Message)
                );
            }

            return MediatR.Unit.Value;
        }
    }
}