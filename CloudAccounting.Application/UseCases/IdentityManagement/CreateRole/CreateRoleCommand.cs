
using MediatR;

namespace CloudAccounting.Application.UseCases.IdentityManagement.CreateRole
{
    public record CreateRoleCommand(string RoleName) : ICommand<Unit>;
}