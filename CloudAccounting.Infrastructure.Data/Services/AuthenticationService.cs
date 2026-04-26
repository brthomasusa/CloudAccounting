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
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IGroupRepository _groupRepository = groupRepository;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<Result<LoginResponseModel>> LoginAsync(string userName, string password)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(userName);

                if (user != null && await _userManager.CheckPasswordAsync(user!, password))
                {
                    return await GetAccessToken(user);
                }

                return Result<LoginResponseModel>.Failure<LoginResponseModel>(
                    new Error("IdentityMgmtRepository.LoginUserAsync", "Invalid login attempt")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<LoginResponseModel>.Failure<LoginResponseModel>(
                    new Error("IdentityMgmtRepository.LoginUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<LoginResponseModel>> LoginWithRefreshTokenAsync(string refreshToken)
        {
            ApplicationUser? user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiresAtUtc <= DateTime.UtcNow)
            {
                return Result<LoginResponseModel>.Failure<LoginResponseModel>(
                    new Error("IdentityMgmtRepository.GetRefreshTokenAsync", "Invalid refresh token")
                );
            }

            return await GetAccessToken(user);
        }

        public async Task<Result<User>> CreateUserWithRoleAsync
        (
            string Email,
            string Password,
            int CompanyCode,
            string RoleName,
            bool IsSystemAdmin,
            bool IsCompanyAdmin
        )
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = Email,
                    NormalizedUserName = Email.ToUpper(),
                    Email = Email,
                    NormalizedEmail = Email.ToUpper(),
                    PhoneNumber = string.Empty,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    LockoutEnabled = false,
                    CompanyCode = CompanyCode,
                    IsAdministrator = IsSystemAdmin || IsCompanyAdmin
                };

                var result = await _userManager.CreateAsync(user, Password);
                await _userManager.AddToRoleAsync(user, RoleName);

                if (!result.Succeeded)
                {
                    string errMsg = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Error creating user with role: {Message}", errMsg);

                    return Result<User>.Failure<User>(new Error("AuthenticationService.CreateUserWithRoleAsync", errMsg));
                }

                Result<User> mapResult = await MapApplicationUserToUser(user, RoleName);

                if (mapResult.IsSuccess)
                {
                    return Result<User>.Success(mapResult.Value);
                }
                else
                {
                    return Result<User>.Failure<User>(mapResult.Error);
                }
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<User>.Failure<User>(
                    new Error("AuthenticationService.CreateUserWithRoleAsync", errMsg)
                );
            }
        }

        private async Task<LoginResponseModel> GetAccessToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

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

            await _userManager.UpdateAsync(user);

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

        private async Task<Result<User>> MapApplicationUserToUser(ApplicationUser user, string RoleName)
        {
            User userProfile = new()
            {
                UserId = user.Email!,
                CompanyCode = user.CompanyCode,
                Admin = user.IsAdministrator ? "Y" : "N",
                GroupTitle = RoleName
            };

            Result<User> groupResult = await _groupRepository.CreateUserAsync(userProfile);

            if (groupResult.IsSuccess)
            {
                return Result<User>.Success(userProfile);
            }
            else
            {
                string errMsg = string.Format("Unable to create user with email {0}: {1}", user.Email, groupResult.Error.Message);
                _logger.LogWarning(errMsg);

                var deleteResult = await _userManager.DeleteAsync(user);

                return Result<User>.Failure<User>(new Error("AuthenticationService.CreateUserWithRoleAsync", errMsg));
            }
        }
    }
}
