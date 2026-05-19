using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Models;
using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Infrastructure.Data.Repositories;

public class CostCenterRepository(
    AppDbContext ctx,
    ILogger<CostCenterRepository> logger
) : ICostCenterRepository
{
    public async Task<Result<List<CostCenter>>> RetrieveAllAsync(int companyCode)
    {
        try
        {
            var dataModels =
                await ctx.CostCenters.Where(cc => cc.CompanyCode == companyCode)
                    .OrderBy(cc => cc.CostCenterCode)
                    .ToListAsync();

            List<CostCenter> costCenters = [];
            dataModels.ForEach(dm => { costCenters.Add(dm.Adapt<CostCenter>()); });

            return costCenters;
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<CostCenter>>(
                new Error("CostCenterRepository.RetrieveAllAsync", errMsg)
            );
        }
    }

    public async Task<Result<CostCenter>> RetrieveAsync(int companyCode, string costCenterCode)
    {
        try
        {
            var dataModel = await ctx.CostCenters
                .SingleOrDefaultAsync(cc => cc.CompanyCode == companyCode && cc.CostCenterCode == costCenterCode);

            if (dataModel == null)
            {
                return Result.Failure<CostCenter>(
                    new Error("CostCenterRepository.RetrieveAsync", "Cost center not found")
                );
            }

            var costCenter = dataModel.Adapt<CostCenter>();
            return Result.Success(costCenter);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<CostCenter>(
                new Error("CostCenterRepository.RetrieveAsync", errMsg)
            );
        }
    }

    public async Task<Result<CostCenter>> CreateAsync(CostCenter c)
    {
        try
        {
            var dataModel = c.Adapt<CostCenterDM>();

            ctx.CostCenters.Add(dataModel);
            await ctx.SaveChangesAsync();

            var createdCostCenter = dataModel.Adapt<CostCenter>();
            return Result.Success(createdCostCenter);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<CostCenter>(
                new Error("CostCenterRepository.CreateAsync", errMsg)
            );
        }
    }

    public async Task<Result<CostCenter>> UpdateAsync(CostCenter c)
    {
        try
        {
            await ctx.CostCenters.Where(cc => cc.CompanyCode == c.CompanyCode && cc.CostCenterCode == c.CostCenterCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(cc => cc.CostCenterTitle, c.CostCenterTitle));

            var dataModel = await ctx.CostCenters
                .SingleOrDefaultAsync(cc => cc.CompanyCode == c.CompanyCode && cc.CostCenterCode == c.CostCenterCode);

            if (dataModel == null)
            {
                return Result.Failure<CostCenter>(
                    new Error("CostCenterRepository.UpdateAsync", "Cost center update failed")
                );
            }

            var updatedCostCenter = dataModel.Adapt<CostCenter>();
            return Result.Success(updatedCostCenter);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<CostCenter>(
                new Error("CostCenterRepository.UpdateAsync", errMsg)
            );
        }
    }

    public async Task<Result<bool>> IsExistingCostCenter(int companyCode, string costCenterCode)
    {
        try
        {
            var exists = await ctx.CostCenters
                .AnyAsync(cc => cc.CompanyCode == companyCode && cc.CostCenterCode == costCenterCode);

            return Result.Success(exists);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<bool>(
                new Error("CostCenterRepository.IsExistingCostCenter", errMsg)
            );
        }
    }

    public async Task<Result<bool>> IsParentWithChildren(int companyCode, string costCenterCode)
    {
        try
        {
            if (costCenterCode.Length == 5)
            {
                return Result.Success(false);
            }

            var hasChildren = await ctx.CostCenters
                .CountAsync(cc => cc.CompanyCode == companyCode && cc.CostCenterCode.StartsWith(costCenterCode));

            return Result.Success(hasChildren > 1);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<bool>(
                new Error("CostCenterRepository.IsParentWithChildren", errMsg)
            );
        }
    }

    public async Task<Result<MediatR.Unit>> DeleteAsync(int companyCode, string costCenterCode)
    {
        try
        {
            await ctx.CostCenters.Where(c => c.CompanyCode == companyCode && c.CostCenterCode == costCenterCode)
                .ExecuteDeleteAsync();

            return Result.Success(MediatR.Unit.Value);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<MediatR.Unit>(
                new Error("CostCenterRepository.DeleteAsync", errMsg)
            );
        }
    }
}