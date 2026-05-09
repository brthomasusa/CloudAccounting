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
        // private readonly IMemoryCache memoryCache = memoryCache;
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

                return Result.Failure<List<GroupsMaster>>(
                    new Error("GroupRepository.RetrieveAllAsync", errMsg)
                );
            }
        }

        public async Task<Result<GroupsMaster>> RetrieveAsync(int groupId)
        {
            try
            {
                if (memoryCache.TryGetValue($"group-{groupId}", out GroupsMasterDM? cachedGroup))
                {
                    return cachedGroup!.Adapt<GroupsMaster>();
                }

                GroupsMasterDM? dataModel = await ctx.GroupsMasters.SingleOrDefaultAsync(g => g.GroupId == groupId);

                if (dataModel != null)
                {
                    memoryCache.Set($"group-{groupId}", dataModel, _cacheEntryOptions);
                    return dataModel.Adapt<GroupsMaster>();
                }

                return Result.Failure<GroupsMaster>(
                    new Error("GroupRepository.RetrieveAsync", $"No group found with ID {groupId}.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<GroupsMaster>(
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

                return Result.Failure<GroupsMaster>(
                    new Error("GroupRepository.RetrieveByGroupNameAsync", $"No group found with name '{groupName}'.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<GroupsMaster>(
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

                memoryCache.Set($"group-{newGroup.GroupId}", newGroup, _cacheEntryOptions);

                return newGroup.Adapt<GroupsMaster>();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<GroupsMaster>(
                    new Error("GroupRepository.CreateAsync", errMsg)
                );
            }
        }

        public async Task<Result<User>> CreateUserAsync(User user)
        {
            try
            {
                Result<GroupsMaster> role = await RetrieveByGroupNameAsync(user.GroupTitle);

                if (!role.IsSuccess)
                {
                    logger.LogWarning("No group found with name '{RoleName}' for user creation.", user.GroupTitle);

                    return Result.Failure<User>(
                        new Error("GroupRepository.CreateUserAsync", $"No group found with name '{user.GroupTitle}'.")
                    );
                }

                UserDM userDm = new()
                {
                    UserId = user.UserId,
                    CompanyCode = user.CompanyCode,
                    CompanyYear = user.CompanyYear,
                    CompanyMonthId = user.CompanyMonthId,
                    GroupId = role.Value.GroupId,
                    Admin = user.Admin
                };

                ctx.UserModels.Add(userDm);
                await ctx.SaveChangesAsync();

                return userDm.Adapt<User>();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<User>(
                    new Error("GroupRepository.CreateUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<User>> RetrieveUserAsync(string email)
        {
            try
            {
                var query = await (from grpMaster in ctx.GroupsMasters
                                   join userModel in ctx.UserModels on grpMaster.GroupId equals userModel.GroupId
                                   join company in ctx.Companies on userModel.CompanyCode equals company.CompanyCode
                                   where userModel.UserId.ToUpper() == email.ToUpper()
                                   select new User
                                   {
                                       UserId = userModel.UserId,
                                       CompanyCode = (int)userModel.CompanyCode!,
                                       CompanyName = company.CompanyName,
                                       CompanyYear = (short)userModel.CompanyYear!,
                                       CompanyMonthId = (byte)userModel.CompanyMonthId!,
                                       CompanyMonthName = GetMonthName((int)userModel.CompanyMonthId!),
                                       GroupId = (short)userModel.GroupId!,
                                       Admin = grpMaster.GroupTitle == "AppAdmin" || grpMaster.GroupTitle == "CompanyAdmin" ? "Y" : "N",
                                       GroupTitle = grpMaster.GroupTitle!
                                   }).SingleOrDefaultAsync();

                if (query != null)
                {
                    return query;
                }

                return Result.Failure<User>(
                    new Error("GroupRepository.RetrieveUserAsync", $"No user found with email '{email}'.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<User>(
                    new Error("GroupRepository.RetrieveUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<User>>> RetrieveAllUserAsync(int companyCode)
        {
            try
            {
                var list = await (from grpMaster in ctx.GroupsMasters
                                  join userModel in ctx.UserModels on grpMaster.GroupId equals userModel.GroupId
                                  join company in ctx.Companies on userModel.CompanyCode equals company.CompanyCode
                                  where userModel.CompanyCode == companyCode
                                  select new User
                                  {
                                      UserId = userModel.UserId,
                                      CompanyCode = (int)userModel.CompanyCode!,
                                      CompanyName = company.CompanyName,
                                      CompanyYear = (short)userModel.CompanyYear!,
                                      CompanyMonthId = (byte)userModel.CompanyMonthId!,
                                      CompanyMonthName = GetMonthName((int)userModel.CompanyMonthId!),
                                      GroupId = (short)userModel.GroupId!,
                                      Admin = grpMaster.GroupTitle == "AppAdmin" || grpMaster.GroupTitle == "CompanyAdmin" ? "Y" : "N",
                                      GroupTitle = grpMaster.GroupTitle!
                                  }).ToListAsync();

                if (list.Count != 0)
                {
                    return list;
                }

                return Result.Failure<List<User>>(
                    new Error("GroupRepository.RetrieveAllUserAsync", "No users found.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<User>>(
                    new Error("GroupRepository.RetrieveAllUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<User>>> RetrieveUserByCompanyAndGroupAsync(int companyCode, int groupId)
        {
            try
            {
                var list = await (from grpMaster in ctx.GroupsMasters
                                  join userModel in ctx.UserModels on grpMaster.GroupId equals userModel.GroupId
                                  join company in ctx.Companies on userModel.CompanyCode equals company.CompanyCode
                                  where userModel.CompanyCode == companyCode && userModel.GroupId == groupId
                                  select new User
                                  {
                                      UserId = userModel.UserId,
                                      CompanyCode = (int)userModel.CompanyCode!,
                                      CompanyName = company.CompanyName,
                                      CompanyYear = (short)userModel.CompanyYear!,
                                      CompanyMonthId = (byte)userModel.CompanyMonthId!,
                                      CompanyMonthName = GetMonthName((int)userModel.CompanyMonthId!),
                                      GroupId = (short)userModel.GroupId!,
                                      Admin = grpMaster.GroupTitle == "AppAdmin" || grpMaster.GroupTitle == "CompanyAdmin" ? "Y" : "N",
                                      GroupTitle = grpMaster.GroupTitle!
                                  }).ToListAsync();

                if (list.Count != 0)
                {
                    return list;
                }

                return Result.Failure<List<User>>(
                    new Error("GroupRepository.RetrieveUserByCompanyAndGroupAsync", "No users found for the specified company and group.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<User>>(
                    new Error("GroupRepository.RetrieveUserByCompanyAndGroupAsync", errMsg)
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

                    return Result.Failure<User>(
                        new Error("GroupRepository.UpdateUserAsync", $"No user found with ID '{user.UserId}'.")
                    );
                }

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

                return Result.Failure<User>(
                    new Error("GroupRepository.UpdateUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<MediatR.Unit>> ChangeUserRoleAssignmentAsync(string email, string newRole, string currentRole)
        {
            try
            {
                // get the user by email (user.UserId is email)
                var userDm = await ctx.UserModels.SingleOrDefaultAsync(u => u.UserId == email);

                if (userDm == null)
                {
                    logger.LogWarning("User with email {Email} not found for role change.", email);

                    return Result.Failure<MediatR.Unit>(
                        new Error("GroupRepository.ChangeUserRoleAssignmentAsync", "User not found")
                    );
                }

                // get the new role
                var newRoleDm = await ctx.GroupsMasters.SingleOrDefaultAsync(g => g.GroupTitle == newRole);

                if (newRoleDm == null)
                {
                    logger.LogWarning("New role {NewRole} not found for user {Email}.", newRole, email);

                    return Result.Failure<MediatR.Unit>(
                        new Error("GroupRepository.ChangeUserRoleAssignmentAsync", "New role not found")
                    );
                }

                // update the user's GroupId to the GroupId of the new role
                userDm.GroupId = newRoleDm.GroupId;
                userDm.Admin = newRole is "AppAdmin" or "CompanyAdmin" ? "Y" : "N";
                
                await ctx.SaveChangesAsync();

                return Result.Success(MediatR.Unit.Value);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<MediatR.Unit>(
                    new Error("GroupRepository.ChangeUserRoleAssignmentAsync", errMsg)
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

                return Result.Failure<bool>(
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

                return Result.Failure<bool>(
                    new Error("GroupRepository.IsValidGroupId", errMsg)
                );
            }
        }

        private static string GetMonthName(int monthNumber)
            => monthNumber switch
            {
                1 => "January",
                2 => "February",
                3 => "March",
                4 => "April",
                5 => "May",
                6 => "June",
                7 => "July",
                8 => "August",
                9 => "September",
                10 => "October",
                11 => "November",
                12 => "December",
                _ => "Invalid month"
            };
    }
}