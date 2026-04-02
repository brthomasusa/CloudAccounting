using CloudAccounting.Infrastructure.Data.IdentityModels;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public interface IIdentityMgmtRepository
    {
        Task<Result<bool>> RegisterUserAsync(Register register);
        Task<Result<string>> LoginUserAsync(Login login);
    }
}