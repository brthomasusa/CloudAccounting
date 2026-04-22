
namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateRoleCommandValidator(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50).WithMessage("Role name cannot exceed 50 characters.")
                .MustAsync(IsUniqueRoleName).WithMessage("There is an existing role with the same name.");
        }

        private async Task<bool> IsUniqueRoleName(string roleName, CancellationToken cancellationToken)
        {
            IdentityRole? existingRole = await _roleManager.FindByNameAsync(roleName);
            return existingRole == null;
        }
    }
}