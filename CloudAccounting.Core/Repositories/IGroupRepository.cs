using CloudAccounting.Core.Models;

namespace CloudAccounting.Core.Repositories
{
    public interface IGroupRepository
    {
        Task<Result<List<GroupsMaster>>> RetrieveAllAsync();

        Task<Result<GroupsMaster>> RetrieveAsync(int groupId);

        Task<Result<GroupsMaster>> RetrieveByGroupNameAsync(string groupName);

        Task<Result<GroupsMaster>> CreateGroupAsync(GroupsMaster group);

        Task<Result<User>> CreateUserAsync(User user);

        Task<Result<User>> RetrieveUserAsync(string email);

        Task<Result<List<User>>> RetrieveAllUserAsync();

        Task<Result<User>> UpdateUserAsync(User user);

        Task<Result<MediatR.Unit>> ChangeUserRoleAssignmentAsync(User user, string newRole, string currentRole);

        Task<Result<bool>> IsUniqueGroupNameForCreate(string groupName);

        Task<Result<bool>> IsValidGroupId(int groupId);
    }
}
