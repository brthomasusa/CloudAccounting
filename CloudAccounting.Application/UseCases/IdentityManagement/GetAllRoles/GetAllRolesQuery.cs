using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetAllRoles
{
    public record class GetAllRolesQuery() : IQuery<List<RoleModel>>;

}