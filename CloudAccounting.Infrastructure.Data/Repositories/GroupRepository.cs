using System.Text.RegularExpressions;
using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Data;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class GroupRepository
    (
        AppDbContext ctx,
        IMemoryCache memoryCache,
        ILogger<GroupRepository> logger
    ) : IGroupRepository
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

        public async Task<Result<List<GroupsMaster>>> RetrieveAllAsync()
        {
            try
            {
                List<GroupsMasterDM> dataModels = await ctx.GroupsMasters.ToListAsync();

                List<GroupsMaster> groups = [];

                dataModels.ForEach(dm =>
                {
                    groups.Add(dm.Adapt<GroupsMaster>());
                });

                return groups;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<List<GroupsMaster>>.Failure<List<GroupsMaster>>(
                    new Error("GroupRepository.RetrieveAllAsync", errMsg)
                );
            }
        }

        public async Task<Result<GroupsMaster>> RetrieveAsync(int groupId)
        {
            try
            {
                if (_memoryCache.TryGetValue($"group-{groupId}", out GroupsMasterDM? cachedGroup))
                {
                    return cachedGroup!.Adapt<GroupsMaster>();
                }

                GroupsMasterDM? dataModel = await ctx.GroupsMasters.SingleOrDefaultAsync(g => g.GroupId == groupId);

                if (dataModel != null)
                {
                    _memoryCache.Set($"group-{groupId}", dataModel, _cacheEntryOptions);
                    return dataModel.Adapt<GroupsMaster>();
                }

                return Result<GroupsMaster>.Failure<GroupsMaster>(
                    new Error("GroupRepository.RetrieveAsync", $"No group found with ID {groupId}.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<GroupsMaster>.Failure<GroupsMaster>(
                    new Error("GroupRepository.RetrieveAsync", errMsg)
                );
            }
        }

        public async Task<Result<GroupsMaster>> RetrieveByGroupNameAsync(string groupName)
        {
            try
            {
                GroupsMasterDM? dataModel = await ctx.GroupsMasters.SingleOrDefaultAsync(g => g.GroupTitle!.ToUpper() == groupName.ToUpper());

                if (dataModel != null)
                {
                    return dataModel.Adapt<GroupsMaster>();
                }

                return Result<GroupsMaster>.Failure<GroupsMaster>(
                    new Error("GroupRepository.RetrieveByGroupNameAsync", $"No group found with name '{groupName}'.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<GroupsMaster>.Failure<GroupsMaster>(
                    new Error("GroupRepository.RetrieveByGroupNameAsync", errMsg)
                );
            }
        }

        public async Task<Result<GroupsMaster>> CreateGroupAsync(GroupsMaster group)
        {
            try
            {
                GroupsMasterDM newGroup = group.Adapt<GroupsMasterDM>();
                ctx.GroupsMasters.Add(newGroup);
                await ctx.SaveChangesAsync();

                _memoryCache.Set($"group-{newGroup.GroupId}", newGroup, _cacheEntryOptions);

                return newGroup.Adapt<GroupsMaster>();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<GroupsMaster>.Failure<GroupsMaster>(
                    new Error("GroupRepository.CreateAsync", errMsg)
                );
            }
        }

        public async Task<Result<User>> CreateUserAsync(User user)
        {
            try
            {
                Result<GroupsMaster> role = await RetrieveByGroupNameAsync(user.RoleName);

                if (!role.IsSuccess)
                {
                    logger.LogWarning("No group found with name '{RoleName}' for user creation.", user.RoleName);

                    return Result<User>.Failure<User>(
                        new Error("GroupRepository.CreateUserAsync", $"No group found with name '{user.RoleName}'.")
                    );
                }

                UserDM userDM = new()
                {
                    UserId = user.UserId,
                    CompanyCode = user.CompanyCode,
                    CompanyYear = user.CompanyYear,
                    CompanyMonthId = user.CompanyMonthId,
                    GroupId = role.Value.GroupId,
                    Admin = user.Admin
                };

                ctx.UserModels.Add(userDM);
                await ctx.SaveChangesAsync();

                return userDM.Adapt<User>();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<User>.Failure<User>(
                    new Error("GroupRepository.CreateUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<User>> UpdateUserAsync(User user)
        {
            try
            {
                UserDM? existingUser = await ctx.UserModels.SingleOrDefaultAsync(u => u.UserId == user.UserId);

                if (existingUser == null)
                {
                    logger.LogWarning("No user found with ID '{UserId}' for update.", user.UserId);

                    return Result<User>.Failure<User>(
                        new Error("GroupRepository.UpdateUserAsync", $"No user found with ID '{user.UserId}'.")
                    );
                }

                // existingUser.CompanyCode = user.CompanyCode;
                existingUser.CompanyYear = user.CompanyYear;
                existingUser.CompanyMonthId = user.CompanyMonthId;
                existingUser.GroupId = user.GroupId;
                existingUser.Admin = user.Admin;

                await ctx.SaveChangesAsync();

                return existingUser.Adapt<User>();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<User>.Failure<User>(
                    new Error("GroupRepository.UpdateUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsUniqueGroupNameForCreate(string groupName)
        {
            try
            {
                bool exists = await ctx.GroupsMasters.AnyAsync(g => g.GroupTitle == groupName);

                return !exists;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("GroupRepository.IsUniqueGroupNameForCreate", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsValidGroupId(int groupId)
        {
            try
            {
                bool exists = await ctx.GroupsMasters.AnyAsync(g => g.GroupId == groupId);

                return exists;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("GroupRepository.IsValidGroupId", errMsg)
                );
            }
        }
    }
}