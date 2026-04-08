using CloudAccounting.Shared.Identity;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public interface IIdentityMgmtRepository
    {
        Task<Result<bool>> RegisterUserAsync(string email, string password, string phoneNumber, int companyCode, bool isAdministrator);

        // Task<Result<string>> RefreshTokenAsync(string token, string refreshToken);
    }
}