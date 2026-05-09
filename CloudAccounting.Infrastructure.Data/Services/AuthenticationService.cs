using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Data;
using CloudAccounting.Infrastructure.Data.Options;
using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Infrastructure.Data.Services
{
    public class AuthenticationService
    (
        UserManager<ApplicationUser> userManager,
        IGroupRepository groupRepository,
        IOptions<JwtOptions> jwtOptions,
        ILogger<AuthenticationService> logger
    )
    {
        // private readonly UserManager<ApplicationUser> userManager = userManager;
        // private readonly IGroupRepository groupRepository = groupRepository;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        // private readonly ILogger<AuthenticationService> logger = logger;

        public async Task<Result<LoginResponseModel>> LoginAsync(string userName, string password)
        {
            try
            {
                ApplicationUser? user = await userManager.FindByEmailAsync(userName);

                if (user != null && await userManager.CheckPasswordAsync(user, password))
                {
                    return await GetAccessToken(user);
                }

                return Result.Failure<LoginResponseModel>(
                    new Error("IdentityMgmtRepository.LoginUserAsync", "Invalid login attempt")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<LoginResponseModel>(
                    new Error("IdentityMgmtRepository.LoginUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<LoginResponseModel>> LoginWithRefreshTokenAsync(string refreshToken)
        {
            ApplicationUser? user = await userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiresAtUtc <= DateTime.UtcNow)
            {
                return Result.Failure<LoginResponseModel>(
                    new Error("IdentityMgmtRepository.GetRefreshTokenAsync", "Invalid refresh token")
                );
            }

            return await GetAccessToken(user);
        }

        public async Task<Result<User>> CreateUserWithRoleAsync
        (
            string email,
            string password,
            int companyCode,
            string roleName
        )
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    PhoneNumber = string.Empty,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    LockoutEnabled = false,
                    CompanyCode = companyCode,
                    IsAdministrator = roleName is "AppAdmin" or "CompanyAdmin"
                };

                var result = await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, roleName);

                if (!result.Succeeded)
                {
                    string errMsg = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogError("Error creating user with role: {Message}", errMsg);

                    return Result.Failure<User>(new Error("AuthenticationService.CreateUserWithRoleAsync", errMsg));
                }

                Result<User> mapResult = await MapApplicationUserToUser(user, roleName);

                if (mapResult.IsSuccess)
                {
                    return Result.Success(mapResult.Value);
                }
                else
                {
                    return Result.Failure<User>(mapResult.Error);
                }
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<User>(
                    new Error("AuthenticationService.CreateUserWithRoleAsync", errMsg)
                );
            }
        }

        private async Task<LoginResponseModel> GetAccessToken(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new("userId", user.Id),
                new("companyCode", user.CompanyCode.ToString()),
                new("userRole", string.Join(",", userRoles)),
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer!,
                expires: DateTime.Now.AddMinutes(_jwtOptions.ExpirationTimeInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret!)),
                SecurityAlgorithms.HmacSha256));

            var refreshTokenValue = GenerateRefreshToken();
            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);

            await userManager.UpdateAsync(user);

            return new LoginResponseModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenExpired = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes).ToUnixTimeSeconds(),
                RefreshToken = refreshTokenValue
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<Result<User>> MapApplicationUserToUser(ApplicationUser user, string roleName)
        {
            User userProfile = new()
            {
                UserId = user.Email!,
                CompanyCode = user.CompanyCode,
                Admin = user.IsAdministrator ? "Y" : "N",
                GroupTitle = roleName
            };

            Result<User> groupResult = await groupRepository.CreateUserAsync(userProfile);

            if (groupResult.IsSuccess)
            {
                return Result.Success(userProfile);
            }
            else
            {
                string errMsg = $"Unable to create user with email {user.Email}: {groupResult.Error.Message}";
                logger.LogWarning(errMsg);

               _ = await userManager.DeleteAsync(user);

                return Result.Failure<User>(new Error("AuthenticationService.CreateUserWithRoleAsync", errMsg));
            }
        }
    }
}
