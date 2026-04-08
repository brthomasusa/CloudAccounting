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

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class IdentityMgmtRepository
    (
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JwtOptions> jwtOptions,
        ILogger<IdentityMgmtRepository> logger
    ) : IIdentityMgmtRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly ILogger<IdentityMgmtRepository> _logger = logger;

        public async Task<Result<bool>> RegisterUserAsync
        (
            string email,
            string password,
            string phoneNumber,
            int companyCode,
            bool isAdministrator
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
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    CompanyCode = companyCode,
                    IsAdministrator = isAdministrator
                };

                var result = await _userManager.CreateAsync(user, password);

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

    }
}