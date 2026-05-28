namespace CloudAccounting.Application.UseCases.Coa.Delete;

public class DeleteChartOfAccountCommandValidator : AbstractValidator<DeleteChartOfAccountCommand>
{
    private readonly IChartOfAccountRepository _coaRepository;

    public DeleteChartOfAccountCommandValidator(IChartOfAccountRepository coaRepository)
    {
        _coaRepository = coaRepository;

        RuleFor(x => x).MustAsync(IsExistingAccount).WithMessage("Account not found for this company.");
    }

    private async Task<bool> IsExistingAccount(DeleteChartOfAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _coaRepository.IsExistingAccount(command.CompanyCode, command.AccountCode);

        return result.Value; // Return true if account exists
    }
}