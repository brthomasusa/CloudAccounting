using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetUsersByCompanyAndGroup;

public record GetUsersByCompanyAndGroupIdQuery
(
    int CompanyCode,
    int GroupId
) : IQuery<List<UserModel>>;