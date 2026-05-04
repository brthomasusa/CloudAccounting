using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetAllUsers;

public class GetAllUsersQueryHandler
(
    IGroupRepository groupRepository,
    ILogger<GetAllUsersQueryHandler> logger
) : IQueryHandler<GetAllUsersQuery, List<UserModel>>
{
    public async Task<Result<List<UserModel>>> Handle
    (
        GetAllUsersQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var getUsersResult = await groupRepository.RetrieveAllUserAsync(query.CompanyCode);

            if (getUsersResult.IsSuccess)
            {
                return getUsersResult.Value.Adapt<List<UserModel>>();
            }

            return Result.Failure<List<UserModel>>(
                new Error("GetAllUsersQueryHandler.Handle", getUsersResult.Error.Message)
            );
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<UserModel>>(
                new Error("GetAllUsersQueryHandler.Handle", errMsg)
            );
        }
    }
}
