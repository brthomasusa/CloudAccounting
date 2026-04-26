

using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetUserById
{
    public class GetUserByIdQueryHandler
    (
        IGroupRepository groupRepository,
        ILogger<GetUserByIdQueryHandler> logger
    ) : IQueryHandler<GetUserByIdQuery, UserModel>
    {
        public async Task<Result<UserModel>> Handle
        (
            GetUserByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<User> getUserResult = await groupRepository.RetrieveUserAsync(query.UserId);

                if (getUserResult.IsSuccess)
                {
                    return getUserResult.Value.Adapt<UserModel>();
                }

                return Result<UserModel>.Failure<UserModel>(
                    new Error("GetUserByIdQueryHandler.Handle", getUserResult.Error.Message)
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<UserModel>.Failure<UserModel>(
                    new Error("GetUserByIdQueryHandler.Handle", errMsg)
                );
            }
        }
    }
}