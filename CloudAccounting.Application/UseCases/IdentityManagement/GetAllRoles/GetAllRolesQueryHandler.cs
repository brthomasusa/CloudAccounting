using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Application.UseCases.IdentityManagement.GetAllRoles
{
    public class GetAllRolesQueryHandler
    (
        IGroupRepository groupRepository,
        ILogger<GetAllRolesQueryHandler> logger
    ) : IQueryHandler<GetAllRolesQuery, List<RoleModel>>
    {
        private readonly IGroupRepository groupRepository = groupRepository;
        private readonly ILogger<GetAllRolesQueryHandler> _logger = logger;

        public async Task<Result<List<RoleModel>>> Handle
        (
            GetAllRolesQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<List<GroupsMaster>> getAllGroupsResult = await groupRepository.RetrieveAllAsync();

                if (getAllGroupsResult.IsSuccess)
                {
                    List<RoleModel> roleModels = [.. getAllGroupsResult.Value
                        .AsQueryable()
                        .ProjectToType<RoleModel>()];

                    return Result<List<RoleModel>>.Success(roleModels);
                }

                return Result<List<RoleModel>>.Failure<List<RoleModel>>(
                    new Error("GetAllRolesQueryHandler.Handle", getAllGroupsResult.Error.Message)
                );

            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<RoleModel>>.Failure<List<RoleModel>>(
                    new Error("GetAllRolesQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}