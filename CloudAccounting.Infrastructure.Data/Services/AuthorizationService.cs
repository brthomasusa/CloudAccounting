using Microsoft.AspNetCore.Identity;
using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Infrastructure.Data.Services
{
    public class AuthorizationService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IGroupRepository groupRepository,
        ILogger<AuthorizationService> logger
    )
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private readonly ILogger<AuthorizationService> _logger = logger;


        public async Task<Result<MediatR.Unit>> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);

            role = await roleManager.FindByNameAsync(roleName);

            GroupsMaster group = new()
            {
                GroupTitle = roleName
            };

            Result<GroupsMaster> result = await groupRepository.CreateGroupAsync(group);

            return Result.Success(MediatR.Unit.Value);
        }

        public async Task<Result<MediatR.Unit>> ChangeUserRoleAssignmentAsync(User user, string newRole, string currentRole)
        {
            try
            {
                // get the user by email (user.UserId is email)
                ApplicationUser? appUser = await _userManager.FindByEmailAsync(user.UserId);

                if (appUser == null)
                {
                    _logger.LogWarning("User with email {Email} not found", user.UserId);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "User not found")
                    );
                }

                // Remove the user from the current role
                var result = await _userManager.RemoveFromRoleAsync(appUser, currentRole);

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to remove user {Email} from role {Role}. Errors: {Errors}", user.UserId, currentRole, result.Errors);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "Failed to remove user from current role")
                    );
                }

                // Add the user to the new role
                var addResult = await _userManager.AddToRoleAsync(appUser, newRole);

                if (!addResult.Succeeded)
                {
                    _logger.LogError("Failed to add user {Email} to role {Role}. Errors: {Errors}", user.UserId, newRole, addResult.Errors);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "Failed to add user to new role")
                    );
                }

                // Update the group assignment in the GL_USER table in thedatabase
                Result<MediatR.Unit> groupResult = await groupRepository.ChangeUserRoleAssignmentAsync(user, newRole, currentRole);

                if (!groupResult.IsSuccess)
                {
                    _logger.LogError("Failed to update group assignment for user {Email}. Errors: {Errors}", user.UserId, groupResult.Error.Message);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "Failed to update group assignment")
                    );
                }

                return Result.Success(MediatR.Unit.Value);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<MediatR.Unit>(
                    new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", errMsg)
                );
            }
        }
    }
}