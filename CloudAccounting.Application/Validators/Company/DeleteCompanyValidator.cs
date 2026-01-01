using FluentValidation;
using CloudAccounting.Application.UseCases.DeleteCompany;

namespace CloudAccounting.Application.Validators.Company;

public class DeleteCompanyValidator : AbstractValidator<DeleteCompanyCommand>
{
    private readonly ICompanyRepository _repository;

    public DeleteCompanyValidator(ICompanyRepository repository)
    {
        _repository = repository;

        RuleFor(company => company.CompanyCode)
                                  .GreaterThan(0).WithMessage("Missing company code.")
                                  .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");
    }

    private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
    {
        Result<CloudAccounting.Core.Models.Company> result =
            await _repository.RetrieveAsync(companyCode, new CancellationToken(), true);

        if (result.IsFailure)
        {
            return false;
        }

        return true;
    }
}