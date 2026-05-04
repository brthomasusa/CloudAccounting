using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetAllUsers;

public record GetAllUsersQuery(int CompanyCode) : IQuery<List<UserModel>>;

