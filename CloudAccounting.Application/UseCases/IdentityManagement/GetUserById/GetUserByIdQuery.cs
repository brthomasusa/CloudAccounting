using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetUserById
{
    public record class GetUserByIdQuery(string UserId) : IQuery<UserModel>;
}