using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateUserWithRole
{
    public class CreateUserWithRoleCommandValidator : AbstractValidator<CreateUserWithRoleCommand>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyRepository _repository;

        public CreateUserWithRoleCommandValidator
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ICompanyRepository repository
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repository = repository;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(async (email, _) =>
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    return user != null;
                }).WithMessage("This email does not exist.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.CompanyCode)
                .GreaterThan(0).WithMessage("Company code must be greater than 0.")
                .MustAsync(async (companyCode, _) =>
                {
                    Result<bool> result = await _repository.IsExistingCompany(companyCode);
                    return result != null && result.Value;
                }).WithMessage("Company code does not exist.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50).WithMessage("Role name cannot exceed 50 characters.")
                .MustAsync(async (roleName, _) =>
                {
                    IdentityRole? existingRole = await roleManager.FindByNameAsync(roleName);
                    return existingRole != null;
                }).WithMessage("Specified role does not exist.");
        }
    }
}