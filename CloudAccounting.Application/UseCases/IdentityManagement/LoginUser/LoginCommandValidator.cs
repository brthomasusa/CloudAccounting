using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Application.UseCases.IdentityManagement.LoginUser
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginCommandValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}