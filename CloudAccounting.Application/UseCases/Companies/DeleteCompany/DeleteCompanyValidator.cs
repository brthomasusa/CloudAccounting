namespace CloudAccounting.Application.UseCases.Companies.DeleteCompany;

public class DeleteCompanyValidator : AbstractValidator<DeleteCompanyCommand>
{
    private readonly ICompanyRepository _repository;

    public DeleteCompanyValidator(ICompanyRepository repository)
    {
        _repository = repository;

        RuleFor(company => company.CompanyCode)
                                  .GreaterThan(0).WithMessage("Missing company code.")
                                  .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.")
                                  .MustAsync(CanCompanyBeDeleted).WithMessage("The company has fiscal years and cannot be deleted.");
    }

    private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
    {
        Result<bool> result = await _repository.IsExistingCompany(companyCode);

        return result.Value;
    }

    private async Task<bool> CanCompanyBeDeleted(int companyCode, CancellationToken cancellationToken)
    {
        Result<bool> result = await _repository.CanCompanyBeDeleted(companyCode);

        return result.Value;
    }
}
