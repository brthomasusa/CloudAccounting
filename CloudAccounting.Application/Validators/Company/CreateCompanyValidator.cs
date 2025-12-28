using FluentValidation;
using CloudAccounting.Application.UseCases.CreateCompany;

namespace CloudAccounting.Application.Validators.Company;

public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
{
    private readonly ICompanyRepository _repository;

    public CreateCompanyValidator(ICompanyRepository repository)
    {
        _repository = repository;

        RuleFor(company => company.CompanyName)
                                  .NotEmpty().WithMessage("The company name is required.")
                                  .MaximumLength(50).WithMessage("Max length of the company name is 50 characters.")
                                  .MustAsync(IsUniqueCompanyName).WithMessage("This company name already exists."); ;

        RuleFor(company => company.Address)
                                  .MaximumLength(100).WithMessage("Max length of the address is 100 characters.");

        RuleFor(company => company.City)
                                  .MaximumLength(15).WithMessage("Max length of the city name is 15 characters.");

        RuleFor(company => company.Zipcode)
                                  .MaximumLength(15).WithMessage("Max length of the zipcode is 15 characters.");

        RuleFor(company => company.Phone)
                                  .MaximumLength(15).WithMessage("Max length of the phone number is 15 characters.");

        RuleFor(company => company.Fax)
                                  .MaximumLength(15).WithMessage("Max length of the fax number is 15 characters.");

        RuleFor(company => company.Currency)
                                  .MaximumLength(3).WithMessage("Max length of the currency code is 3 characters.");
    }

    private async Task<bool> IsUniqueCompanyName(string companyName, CancellationToken cancellationToken)
    {
        Result<bool> result = await _repository.IsUniqueCompanyName(companyName);
        return result.Value;
    }
}
