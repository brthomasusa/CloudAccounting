namespace CloudAccounting.Application.UseCases.Coa.Update;

public class UpdateChartOfAccountCommandValidator : AbstractValidator<UpdateChartOfAccountCommand>
{
    private readonly IChartOfAccountRepository _coaRepository;
    private readonly ICostCenterRepository _costCenterRepository;

    public UpdateChartOfAccountCommandValidator(IChartOfAccountRepository coaRepository,
        ICostCenterRepository costCenterRepository)
    {
        _coaRepository = coaRepository;
        _costCenterRepository = costCenterRepository;

        RuleFor(x => x).MustAsync(IsExistingAccount).WithMessage("Account not found for this company.");

        RuleFor(x => x.AccountTitle).NotEmpty().MaximumLength(50)
            .WithMessage("Account title is required and must not exceed 50 characters.");

        RuleFor(x => x.AccountType).NotEmpty().WithMessage("Account type is required.");

        RuleFor(x => x)
            .MustAsync(IsExistingCostCenter)
            .WithMessage("The company code is not valid.");
    }

    private async Task<bool> IsExistingAccount(UpdateChartOfAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _coaRepository.IsExistingAccount(command.CompanyCode, command.AccountCode);

        return result.Value; // Return true if account exists
    }

    private async Task<bool> IsExistingCostCenter(UpdateChartOfAccountCommand command,
        CancellationToken cancellationToken)

    {
        var result = await _costCenterRepository.IsExistingCostCenter(command.CompanyCode, command.CostCenterCode);

        return result.Value;
    }
}