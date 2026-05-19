
namespace CloudAccounting.Core.Repositories;

public interface ICostCenterRepository
{
        Task<Result<List<CostCenter>>> RetrieveAllAsync(int companyCode);

        Task<Result<CostCenter>> RetrieveAsync(int companyCode, string costCenterCode);

        Task<Result<CostCenter>> CreateAsync(CostCenter c);

        Task<Result<CostCenter>> UpdateAsync(CostCenter c);

        Task<Result<MediatR.Unit>> DeleteAsync(int companyCode, string costCenterCode);

        Task<Result<bool>> IsExistingCostCenter(int companyCode, string costCenterCode);

        Task<Result<bool>> IsParentWithChildren(int companyCode, string costCenterCode);
}