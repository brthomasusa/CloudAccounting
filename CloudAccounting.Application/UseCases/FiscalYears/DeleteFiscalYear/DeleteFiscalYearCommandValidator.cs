namespace CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;

public class DeleteFiscalYearCommandValidator : AbstractValidator<DeleteFiscalYearCommand>
{
    private readonly IFiscalYearRepository _repository;

    public DeleteFiscalYearCommandValidator(IFiscalYearRepository repository)
    {
        _repository = repository;

        RuleFor(fiscalYear => fiscalYear.CompanyCode)
                                        .GreaterThan(0).WithMessage("Missing company code.")
                                        .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");

        RuleFor(fiscalYear => fiscalYear.FiscalYear)
                                        .GreaterThan(0).WithMessage("Missing fiscal year.")
                                        .MustAsync(ValidateFiscalYearCanBeDeleted).WithMessage("This fiscal year can't be deleted because it has transactions.");
    }

    private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
    {
        Result<bool> result = await _repository.IsValidCompanyCode(companyCode);

        return result.Value;
    }

    private async Task<bool> ValidateFiscalYearCanBeDeleted(DeleteFiscalYearCommand command, int fiscalYearNumber, CancellationToken cancellationToken)
    {
        Result<bool> result = await _repository.CanFiscalYearBeDeleted(command.CompanyCode, command.FiscalYear);

        return result.Value;
    }
}
