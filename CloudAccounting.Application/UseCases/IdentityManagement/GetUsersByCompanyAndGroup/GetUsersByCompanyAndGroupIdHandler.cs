using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetUsersByCompanyAndGroup;

public class GetUsersByCompanyAndGroupIdHandler
(
    IGroupRepository groupRepository,
    ILogger<GetUsersByCompanyAndGroupIdHandler> logger
) : IQueryHandler<GetUsersByCompanyAndGroupIdQuery, List<UserModel>>
{
    public async Task<Result<List<UserModel>>> Handle
    (
        GetUsersByCompanyAndGroupIdQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var getUsersResult = await groupRepository.RetrieveUserByCompanyAndGroupAsync(query.CompanyCode, query.GroupId);

            if (getUsersResult.IsSuccess)
            {
                return getUsersResult.Value.Adapt<List<UserModel>>();
            }

            return Result.Failure<List<UserModel>>(
                new Error("GetUsersByCompanyAndGroupIdHandler.Handle", getUsersResult.Error.Message)
            );
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<UserModel>>(
                new Error("GetUsersByCompanyAndGroupIdHandler.Handle", errMsg)
            );
        }
    }
}
