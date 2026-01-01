using FluentValidation;
using CloudAccounting.Application.UseCases.UpdateCompany;

namespace CloudAccounting.Application.Validators.Company
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand>
    {
        private readonly ICompanyRepository _repository;

        public UpdateCompanyValidator(ICompanyRepository repository)
        {
            _repository = repository;

            RuleFor(company => company.CompanyCode)
                                      .GreaterThan(0).WithMessage("Missing company code.")
                                      .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");

            RuleFor(company => company.CompanyName)
                                      .NotEmpty().WithMessage("The company name is required.")
                                      .MaximumLength(50).WithMessage("Max length of the company name is 50 characters.")
                                      .MustAsync(CheckCompanyName!).WithMessage("Another company already has this name.");

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

        private async Task<bool> CheckCompanyName(UpdateCompanyCommand command, string companyName, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.IsUniqueCompanyNameForUpdate(command.CompanyCode, companyName);
            return result.Value;
        }
    }
}