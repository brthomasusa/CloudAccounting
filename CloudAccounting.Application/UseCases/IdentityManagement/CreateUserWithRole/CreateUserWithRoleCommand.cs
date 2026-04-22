using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateUserWithRole
{
    public record class CreateUserWithRoleCommand
    (
        string Email,
        string Password,
        int CompanyCode,
        string RoleName,
        bool IsSystemAdmin,
        bool IsCompanyAdmin
    ) : ICommand<MediatR.Unit>;
}