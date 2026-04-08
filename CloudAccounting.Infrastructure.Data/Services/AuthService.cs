using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CloudAccounting.Infrastructure.Data.Data;
using CloudAccounting.Infrastructure.Data.Options;
using CloudAccounting.Shared.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CloudAccounting.Infrastructure.Data.Services
{
    public class AuthService
    (
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JwtOptions> jwtOptions,
        ILogger<AuthService> logger
    )
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly ILogger<AuthService> _logger = logger;

        public async Task<Result<LoginResponseModel>> LoginAsync(string userName, string password)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(userName);

                if (user != null && await _userManager.CheckPasswordAsync(user!, password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new(JwtRegisteredClaimNames.Name, user.UserName!),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new(JwtRegisteredClaimNames.Email, user.Email!),
                        new("userId", user.Id)
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

                    return Result<LoginResponseModel>.Success(new LoginResponseModel
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        TokenExpired = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds(),
                        RefreshToken = refreshTokenValue
                    });
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

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
