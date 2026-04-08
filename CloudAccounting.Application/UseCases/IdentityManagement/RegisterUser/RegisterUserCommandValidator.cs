using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Application.UseCases.IdentityManagement.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyRepository _repository;

        public RegisterUserCommandValidator(UserManager<ApplicationUser> userManager, ICompanyRepository repository)
        {
            _userManager = userManager;
            _repository = repository;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(IsUniqueEmail).WithMessage("This email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.CompanyCode)
                .GreaterThan(0).WithMessage("Missing company code.")
                .MustAsync(ValidateCompanyCode).WithMessage("The company code is not valid.");
        }

        private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.IsExistingCompany(companyCode);

            return result.Value;
        }
    }
}