
namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateUserWithRole
{
    public record CreateUserWithRoleCommand
    (
        string Email,
        string Password,
        int CompanyCode,
        string RoleName
    ) : ICommand<MediatR.Unit>;
}