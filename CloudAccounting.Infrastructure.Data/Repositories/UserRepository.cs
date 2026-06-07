
using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Infrastructure.Data.Repositories;

public class UserRepository(
    AppDbContext db,
    ILogger<UserRepository> logger
) : IUserRepository
{
    public async Task<Result> UpdateAllUsersFiscalPeriodAsync(int companyCode, short companyYear, byte companyMonthId)
    {
        try
        {
            await db.UserModels.Where(u => u.CompanyCode == companyCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.CompanyYear, companyYear)
                    .SetProperty(u => u.CompanyMonthId, companyMonthId));

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure(new Error("UserRepository.UpdateAllUsersFiscalPeriodAsync", errMsg));
        }
    }
}