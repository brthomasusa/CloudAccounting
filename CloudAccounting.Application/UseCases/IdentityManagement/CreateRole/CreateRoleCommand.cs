
namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateRole
{
    public record class CreateRoleCommand(string RoleName) : ICommand<MediatR.Unit>;
}