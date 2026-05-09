
namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateUserWithRole
{
    public record CreateUserWithRoleCommand
    (
        string Email,
        string Password,
        int CompanyCode,
        string RoleName,
        bool IsSystemAdmin,
        bool IsCompanyAdmin
    ) : ICommand<MediatR.Unit>;
}