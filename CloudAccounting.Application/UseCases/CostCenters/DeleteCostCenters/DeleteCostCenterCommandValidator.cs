namespace CloudAccounting.Application.UseCases.CostCenters.DeleteCostCenters;

public class DeleteCostCenterCommandValidator : AbstractValidator<DeleteCostCenterCommand>
{
    private readonly ICostCenterRepository _repository;

    public DeleteCostCenterCommandValidator(ICostCenterRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.CompanyCode)
            .GreaterThan(0)
            .MustAsync(IsExistingCostCenter).WithMessage("This cost center was not found.")
            .MustAsync(IsParentWithChildren)
            .WithMessage("This cost center has child cost centers and cannot be deleted.");

        RuleFor(x => x.CostCenterCode)
            .NotEmpty()
            .Must(val => (val is { Length: 2 } && val.All(char.IsDigit)) ||
                         (val is { Length: 5 } && val.All(char.IsDigit)))
            .WithMessage("'{CostCenterCode}' must be a 2-digit or 5-digit number.");
    }

    private async Task<bool> IsExistingCostCenter(DeleteCostCenterCommand command, int companyCode,
        CancellationToken cancellationToken)
    {
        var result = await _repository.IsExistingCostCenter(command.CompanyCode, command.CostCenterCode);

        return result.Value;
    }

    private async Task<bool> IsParentWithChildren(DeleteCostCenterCommand command, int companyCode,
        CancellationToken cancellationToken)
    {
        var result = await _repository.IsParentWithChildren(companyCode, command.CostCenterCode);

        return !result.Value;
    }
}