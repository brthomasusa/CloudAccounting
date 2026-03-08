namespace CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear;

public class CreateFiscalYearCommandValidator : AbstractValidator<CreateFiscalYearCommand>
{
    private readonly IFiscalYearRepository _repository;

    public CreateFiscalYearCommandValidator(IFiscalYearRepository repository)
    {
        _repository = repository;

        RuleFor(fiscalYear => fiscalYear.CompanyCode)
                                        .GreaterThan(0).WithMessage("Missing company code.")
                                        .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");

        RuleFor(fiscalYear => fiscalYear.FiscalYearNumber)
                                        .GreaterThan(0).WithMessage("Missing fiscal year.")
                                        .MustAsync(ValidateFiscalYearNumber).WithMessage("The fiscal year number already exists for this company.");

        RuleFor(fiscalYear => fiscalYear.StartDate)
                                        .NotNull().WithMessage("Missing fiscal year start date.")
                                        .MustAsync(ValidateFiscalYearStartDate).WithMessage("The start date overlaps with an existing fiscal year for this company.");
    }

    private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
    {
        Result<bool> result = await _repository.IsValidCompanyCode(companyCode);

        return result.Value;
    }

    private async Task<bool> ValidateFiscalYearNumber(CreateFiscalYearCommand command, int fiscalYear, CancellationToken cancellationToken)
    {
        Result<bool> result = await _repository.IsUniqueFiscalYearNumber(command.CompanyCode, command.FiscalYearNumber);

        return result.Value;
    }

    private async Task<bool> ValidateFiscalYearStartDate(CreateFiscalYearCommand command, DateTime startDate, CancellationToken cancellationToken)
    {
        Result<DateTime> result = await _repository.EarliestNextFiscalYearStartDate(command.CompanyCode);

        return startDate >= result.Value;
    }
}
