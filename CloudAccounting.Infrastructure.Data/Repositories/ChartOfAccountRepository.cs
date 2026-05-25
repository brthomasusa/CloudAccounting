using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Models;
using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Infrastructure.Data.Repositories;

public class ChartOfAccountRepository(
    AppDbContext ctx,
    ILogger<ChartOfAccountRepository> logger
) : IChartOfAccountRepository
{
    public async Task<Result<PagedResponse<ChartOfAccounts>>> RetrieveAllAsync
    (
        int pageNumber,
        int pageSize,
        int companyCode
    )
    {
        try
        {
            var query = ctx.ChartOfAccounts.AsNoTracking();
            var totalRecords = await query.CountAsync();

            var dataModels = await query
                .OrderBy(p => p.AccountCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var results = new List<ChartOfAccounts>();
            dataModels.ForEach(dm => results.Add(dm.Adapt<ChartOfAccounts>()));

            var pagedResponse = new PagedResponse<ChartOfAccounts>(results, pageNumber, pageSize, totalRecords);

            return Result.Success(pagedResponse);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<PagedResponse<ChartOfAccounts>>(new Error("ChartOfAccountRepository.RetrieveAllAsync",
                errMsg));
        }
    }

    public async Task<Result<PagedResponse<ChartOfAccounts>>> RetrieveAllAsync
    (
        int pageNumber,
        int pageSize,
        int companyCode,
        string? searchTerm
    )
    {
        try
        {
            var query = ctx.ChartOfAccounts.AsNoTracking()
                .Where(c => c.CompanyCode == companyCode);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchPattern = $"{searchTerm}%";
                // query = query.Where(c => c.AccountCode.Contains(searchTerm));
                query = query.Where(c => EF.Functions.Like(c.AccountCode, searchPattern));
            }

            var totalRecords = await query.CountAsync();

            var dataModels = await query
                .OrderBy(p => p.AccountCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var results = new List<ChartOfAccounts>();
            dataModels.ForEach(dm => results.Add(dm.Adapt<ChartOfAccounts>()));

            var pagedResponse = new PagedResponse<ChartOfAccounts>(results, pageNumber, pageSize, totalRecords);

            return Result.Success(pagedResponse);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<PagedResponse<ChartOfAccounts>>(new Error("ChartOfAccountRepository.RetrieveAllAsync",
                errMsg));
        }
    }

    public async Task<Result<ChartOfAccounts>> RetrieveAsync(int companyCode, string accountCode)
    {
        try
        {
            var dataModel = await ctx.ChartOfAccounts
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.CompanyCode == companyCode && c.AccountCode == accountCode);

            if (dataModel == null)
            {
                return Result.Failure<ChartOfAccounts>(new Error("ChartOfAccountRepository.RetrieveAsync",
                    "Account not found"));
            }

            return Result.Success(dataModel.Adapt<ChartOfAccounts>());
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<ChartOfAccounts>(new Error("ChartOfAccountRepository.RetrieveAsync", errMsg));
        }
    }

    public async Task<Result<ChartOfAccounts>> CreateAsync(ChartOfAccounts c)
    {
        try
        {
            var dataModel = c.Adapt<ChartOfAccountsDM>();

            ctx.ChartOfAccounts.Add(dataModel);
            await ctx.SaveChangesAsync();

            return Result.Success(dataModel.Adapt<ChartOfAccounts>());
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<ChartOfAccounts>(new Error("ChartOfAccountRepository.CreateAsync", errMsg));
        }
    }

    public async Task<Result<ChartOfAccounts>> UpdateAsync(ChartOfAccounts c)
    {
        try
        {
            await ctx.ChartOfAccounts.Where(x => x.CompanyCode == c.CompanyCode && x.AccountCode == c.AccountCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.AccountTitle, c.AccountTitle)
                    .SetProperty(x => x.AccountType, c.AccountType)
                    .SetProperty(x => x.CostCenterCode, c.CostCenterCode)
                );

            var dataModel = await ctx.ChartOfAccounts
                .SingleOrDefaultAsync(x => x.CompanyCode == c.CompanyCode && x.AccountCode == c.AccountCode);

            if (dataModel == null)
            {
                return Result.Failure<ChartOfAccounts>(new Error("ChartOfAccountRepository.UpdateAsync",
                    "Account update failed"));
            }

            return Result.Success(dataModel.Adapt<ChartOfAccounts>());
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<ChartOfAccounts>(new Error("ChartOfAccountRepository.UpdateAsync", errMsg));
        }
    }

    public async Task<Result> DeleteAsync(int companyCode, string accountCode)
    {
        try
        {
            await ctx.ChartOfAccounts.Where(x => x.CompanyCode == companyCode && x.AccountCode == accountCode)
                .ExecuteDeleteAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure(new Error("ChartOfAccountRepository.DeleteAsync", errMsg));
        }
    }

    public async Task<Result<bool>> IsExistingAccount(int companyCode, string accountCode)
    {
        try
        {
            var exists = await ctx.ChartOfAccounts
                .AnyAsync(x => x.CompanyCode == companyCode && x.AccountCode == accountCode);

            return Result.Success(exists);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<bool>(new Error("ChartOfAccountRepository.IsExistingAccount", errMsg));
        }
    }

    public async Task<Result<bool>> IsParentWithChildren(int companyCode, string accountCode)
    {
        try
        {
            // If the account code length indicates leaf (e.g., 4), treat as no children
            if (accountCode.Length <= 4)
            {
                return Result.Success(false);
            }

            var count = await ctx.ChartOfAccounts.CountAsync(x =>
                x.CompanyCode == companyCode && x.AccountCode.StartsWith(accountCode));

            return Result.Success(count > 1);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<bool>(new Error("ChartOfAccountRepository.IsParentWithChildren", errMsg));
        }
    }
}
