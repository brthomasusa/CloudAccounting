using FluentValidation;
using CloudAccounting.Application.UseCases.Company.CreateFiscalYear;

namespace CloudAccounting.Application.Validators.Company
{
    public class CreateFiscalYearCommandValidator : AbstractValidator<CreateFiscalYearCommand>
    {
        private readonly ICompanyReadRepository _readRepository;
        public CreateFiscalYearCommandValidator(ICompanyReadRepository readRepository)
        {
            _readRepository = readRepository;

            RuleFor(company => company.CompanyCode)
                                      .GreaterThan(0).WithMessage("Missing company code.")
                                      .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");

            RuleFor(company => company.FiscalYear)
                                      .GreaterThan(0).WithMessage("Missing fiscal year.")
                                      .Must(y => y >= 2000 && y <= DateTime.Now.Year + 1).WithMessage("A valid fiscal year is between 2000 and next year.");

            RuleFor(company => company.StartMonthNumber)
                                      .GreaterThan(0).WithMessage("What is the first month (month number) in the fiscal year.")
                                      .Must(m => m >= 1 && m <= 12).WithMessage("Start month number should be between 1 and 12.");
        }

        private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
        {
            Result<bool> result = await _readRepository.IsExistingCompany(companyCode);

            return result.Value;
        }


    }
}