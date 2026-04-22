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
    }
}