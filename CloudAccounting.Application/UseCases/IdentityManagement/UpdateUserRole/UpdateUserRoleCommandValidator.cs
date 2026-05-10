using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserRole;

public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserRoleCommandValidator
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MustAsync(async (email, _) =>
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    return user != null;
                }).WithMessage("This email does not exist.");            
            
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50).WithMessage("Role name cannot exceed 50 characters.")
                .MustAsync(async (roleName, _) =>
                {
                    IdentityRole? existingRole = await _roleManager.FindByNameAsync(roleName);
                    return existingRole != null;
                }).WithMessage("Specified role does not exist.");
        }
}