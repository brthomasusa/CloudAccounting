namespace CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserFiscalPeriod;

public class UpdateUserFiscalPeriodCommandValidator : AbstractValidator<UpdateUserFiscalPeriodCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IFiscalYearRepository _fiscalYearRepository;

    public UpdateUserFiscalPeriodCommandValidator
    (
        ICompanyRepository companyRepository,
        IFiscalYearRepository fiscalYearRepository
    )
    {
        _companyRepository = companyRepository;
        _fiscalYearRepository = fiscalYearRepository;

        RuleFor(x => x.CompanyCode)
            .GreaterThan(0).WithMessage("Missing company code.")
            .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");

        RuleFor(x => x.CompanyYear).GreaterThan((short)0)
            .MustAsync(ValidateFiscalYearNumber)
            .WithMessage("The fiscal year number does not exist for this company.");

        RuleFor(x => x.CompanyMonthId).InclusiveBetween((byte)1, (byte)12);
    }

    private async Task<bool> ValidateFiscalYearNumber(UpdateUserFiscalPeriodCommand command, short fiscalYear,
        CancellationToken cancellationToken)
    {
        Result<bool> result =
            await _fiscalYearRepository.IsValidFiscalYearNumber(command.CompanyCode, command.CompanyYear);

        return result.Value;
    }

    private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
    {
        Result<bool> result = await _companyRepository.IsExistingCompany(companyCode);

        return result.Value;
    }
}