namespace CloudAccounting.Application.UseCases.CostCenters.UpdateCostCenter;

public class UpdateCostCenterCommandValidator : AbstractValidator<UpdateCostCenterCommand>
{
    private readonly ICostCenterRepository _repository;

    public UpdateCostCenterCommandValidator(ICostCenterRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.CostCenterCode)
            .NotEmpty()
            .Must(val => (val is { Length: 2 } && val.All(char.IsDigit)) || 
                                 (val is { Length: 5 } && val.All(char.IsDigit)) )
            .WithMessage("'{CostCenterCode}' must be a 2-digit or 5-digit number.");        

        RuleFor(x => x.CompanyCode)
			.GreaterThan(0)
			.MustAsync(IsExistingCostCenter).WithMessage("This cost center was not found.");        
        
        RuleFor(x => x.CostCenterTitle)
            .NotEmpty()
            .MaximumLength(25);

    }

    private async Task<bool> IsExistingCostCenter(UpdateCostCenterCommand command, int companyCode, CancellationToken cancellationToken)
    {
        var result = await _repository.IsExistingCostCenter(command.CompanyCode, command.CostCenterCode);
        return result.Value;
    }    
}