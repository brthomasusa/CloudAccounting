namespace CloudAccounting.Core.Repositories;

public interface IChartOfAccountRepository
{
	Task<Result<List<ChartOfAccounts>>> RetrieveAllAsync(int companyCode);

	Task<Result<ChartOfAccounts>> RetrieveAsync(int companyCode, string accountCode);

	Task<Result<ChartOfAccounts>> CreateAsync(ChartOfAccounts c);

	Task<Result<ChartOfAccounts>> UpdateAsync(ChartOfAccounts c);

	Task<Result> DeleteAsync(int companyCode, string accountCode);

	Task<Result<bool>> IsExistingAccount(int companyCode, string accountCode);

	Task<Result<bool>> IsParentWithChildren(int companyCode, string accountCode);
}