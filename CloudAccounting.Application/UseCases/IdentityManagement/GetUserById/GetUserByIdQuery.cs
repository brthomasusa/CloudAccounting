using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetUserById
{
    public record GetUserByIdQuery(string UserId) : IQuery<UserModel>;
}