namespace CloudAccounting.Core.Repositories;

public interface IUserRepository
{
    Task<Result> UpdateAllUsersFiscalPeriodAsync(int companyCode, short companyYear, byte companyMonthId);
}