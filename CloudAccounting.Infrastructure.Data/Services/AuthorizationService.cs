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

        public async Task<Result<MediatR.Unit>> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);

            GroupsMaster group = new()
            {
                GroupTitle = roleName
            };

            Result<GroupsMaster> result = await groupRepository.CreateGroupAsync(group);

            if (!result.IsSuccess)
            {
                logger.LogError("Failed to create group for role {RoleName}. Errors: {Errors}", roleName, result.Error.Message);

                return Result.Failure<MediatR.Unit>(
                    new Error("AuthorizationService.CreateRoleAsync", "Failed to create group for role")
                );
            }

            return Result.Success(MediatR.Unit.Value);
        }

        public async Task<Result<MediatR.Unit>> ChangeUserRoleAssignmentAsync(string email, string newRole, string currentRole)
        {
            try
            {
                // get the user by email (user.UserId is email)
                ApplicationUser? appUser = await userManager.FindByEmailAsync(email);

                if (appUser == null)
                {
                    logger.LogWarning("User with email {Email} not found", email);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "User not found")
                    );
                }

                // Remove the user from the current role
                var result = await userManager.RemoveFromRoleAsync(appUser, currentRole);

                if (!result.Succeeded)
                {
                    logger.LogError("Failed to remove user {Email} from role {Role}. Errors: {Errors}", email, currentRole, result.Errors);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "Failed to remove user from current role")
                    );
                }

                // Add the user to the new role
                var addResult = await userManager.AddToRoleAsync(appUser, newRole);

                if (!addResult.Succeeded)
                {
                    logger.LogError("Failed to add user {Email} to role {Role}. Errors: {Errors}", email, newRole, addResult.Errors);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "Failed to add user to new role")
                    );
                }

                // Update the group assignment in the GL_USER table in thedatabase
                Result<MediatR.Unit> groupResult = await groupRepository.ChangeUserRoleAssignmentAsync(email, newRole, currentRole);

                if (!groupResult.IsSuccess)
                {
                    logger.LogError("Failed to update group assignment for user {Email}. Errors: {Errors}", email, groupResult.Error.Message);

                    return Result.Failure<MediatR.Unit>(
                        new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", "Failed to update group assignment")
                    );
                }

                return Result.Success(MediatR.Unit.Value);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<MediatR.Unit>(
                    new Error("AuthorizationService.ChangeUserRoleAssignmentAsync", errMsg)
                );
            }
        }
    }
}