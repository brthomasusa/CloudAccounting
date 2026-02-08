using CloudAccounting.Application.UseCases.Company.DeleteFiscalYear;

namespace CloudAccounting.Application.Validators.Company
{
    public class DeleteFiscalYearCommandValidator : AbstractValidator<DeleteFiscalYearCommand>
    {
        private readonly ICompanyReadRepository _repository;

        public DeleteFiscalYearCommandValidator(ICompanyReadRepository repository)
        {
            _repository = repository;

            RuleFor(company => company.CompanyCode)
                                      .GreaterThan(0).WithMessage("Missing company code.")
                                      .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");

            RuleFor(company => company.FiscalYear)
                                      .GreaterThan(0).WithMessage("Missing fiscal year.")
                                      .MustAsync(CanFiscalYearBeDeleted!).WithMessage("The fiscal year has transactions and can not be deleted.");
        }

        private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.IsExistingCompany(companyCode);

            return result.Value;
        }

        private async Task<bool> CanFiscalYearBeDeleted(DeleteFiscalYearCommand command, int fiscalYear, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.CanCompanyFiscalYearBeDeleted(command.CompanyCode, fiscalYear);
            return result.Value;
        }
    }
}