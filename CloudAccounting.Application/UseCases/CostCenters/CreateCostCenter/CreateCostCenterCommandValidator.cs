namespace CloudAccounting.Application.UseCases.CostCenters.CreateCostCenter;

public class CreateCostCenterCommandValidator : AbstractValidator<CreateCostCenterCommand>
{
    private readonly ICostCenterRepository _repository;

    public CreateCostCenterCommandValidator(ICostCenterRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.CompanyCode).Equals(0);

        RuleFor(x => x.CostCenterCode)
            .NotEmpty()
            .Must(val => (val is { Length: 2 } && val.All(char.IsDigit)) || 
                                 (val is { Length: 5 } && val.All(char.IsDigit)) )
            .WithMessage("'{CostCenterCode}' must be a 2-digit or 5-digit number.");

        RuleFor(x => x.CostCenterTitle).NotEmpty().MaximumLength(25);

        // RuleFor(x => x.CostCenterLevel).InclusiveBetween((byte)1, (byte)5);
    } 

    private async Task<bool> IsExistingCostCenter(CreateCostCenterCommand command, int companyCode, CancellationToken cancellationToken)
    {
        var result = await _repository.IsExistingCostCenter(command.CompanyCode, command.CostCenterCode);
        return result.Value;
    }    
}