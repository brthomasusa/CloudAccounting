using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CloudAccounting.Infrastructure.Data.Data;
using CloudAccounting.Infrastructure.Data.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class IdentityMgmtRepository
    (
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<IdentityMgmtRepository> logger
    ) : IIdentityMgmtRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ILogger<IdentityMgmtRepository> _logger = logger;

        public async Task<Result<bool>> RegisterUserAsync(Register register)
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = register.Email,
                    NormalizedUserName = register.Email.ToUpper(),
                    Email = register.Email,
                    NormalizedEmail = register.Email.ToUpper(),
                    PhoneNumber = register.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                };

                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    return Result<bool>.Success(true);
                }
                else
                {
                    string error = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result<bool>.Failure<bool>(new Error("IdentityMgmtRepository.RegisterUserAsync", error));
                }
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("IdentityMgmtRepository.RegisterUserAsync", errMsg)
                );
            }
        }

        public async Task<Result<string>> LoginUserAsync(Login login)
        {
            try
            {
                string issuer = "CloudAccountingAPI";
                int expiryMinutes = 60;
                string key = "2d5785e2-f7b4-4b92-a6c0-ed3988030dbc-f62ef9c6-675c-46f3-96e9-6cb983eab39f";

                ApplicationUser? user = await _userManager.FindByEmailAsync(login.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user!, login.Password))
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
                        issuer: issuer,
                        expires: DateTime.Now.AddMinutes(expiryMinutes),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        SecurityAlgorithms.HmacSha256));

                    return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
                }
                return Result<string>.Failure<string>(
                    new Error("IdentityMgmtRepository.LoginUserAsync", "Invalid login attempt")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<string>.Failure<string>(
                    new Error("IdentityMgmtRepository.LoginUserAsync", errMsg)
                );
            }
        }
    }
}