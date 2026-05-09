namespace CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserRole;

public record UpdateUserRoleCommand
(
    string Email,
    string RoleName
) : ICommand<MediatR.Unit>;
