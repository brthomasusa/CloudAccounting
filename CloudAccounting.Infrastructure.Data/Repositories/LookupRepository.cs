using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class LookupRepository(
        AppDbContext context,
        ILogger<LookupRepository> logger
    ) : ILookupRepository
    {
        public async Task<Result<List<CompanyLookupItem>>> RetrieveAllAsync()
        {
            try
            {
                List<CompanyLookupItem> companies = await context.Companies
                    .Select(c => new CompanyLookupItem
                    {
                        CompanyCode = c.CompanyCode,
                        CompanyName = c.CompanyName
                    })
                    .ToListAsync();

                return companies;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<CompanyLookupItem>>(
                    new Error("LookupRepository.RetrieveAllAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<CostCenterLookupItem>>> RetrieveCostCentersAsync(int companyCode)
        {
            try
            {
                List<CostCenterLookupItem> costCenters = await context.CostCenters
                    .Where(cc => cc.CompanyCode == companyCode && cc.CostCenterLevel == 2)
                    .OrderBy(cc => cc.CostCenterCode)
                    .Select(cc => new CostCenterLookupItem
                    {
                        CostCenterCode = cc.CostCenterCode,
                        CostCenterTitle = cc.CostCenterTitle
                    })
                    .ToListAsync();

                return costCenters;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<CostCenterLookupItem>>(
                    new Error("LookupRepository.RetrieveCostCentersAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<FiscalYearLookupItem>>> RetrieveFiscalYearsAsync(int companyCode)
        {
            try
            {
                var uniqueYears = await context.FiscalYears
                    .Where(fy => fy.CompanyCode == companyCode && fy.YearClosed == false)
                    .OrderBy(fy => fy.CompanyYear)
                    .Select(fy => new FiscalYearLookupItem
                    {
                        CompanyYear = fy.CompanyYear
                    })
                    .Distinct()
                    .ToListAsync();

                return uniqueYears;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<FiscalYearLookupItem>>(
                    new Error("LookupRepository.RetrieveFiscalYearsAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<FiscalPeriodLookupItem>>> RetrieveFiscalPeriodsAsync(int companyCode,
            int companyYear)
        {
            try
            {
                List<FiscalPeriodLookupItem> fiscalPeriods = await context.FiscalYears
                    .Where(fp =>
                        fp.CompanyCode == companyCode && fp.CompanyYear == companyYear && fp.MonthClosed == false)
                    .OrderBy(fp => fp.CompanyMonthId)
                    .Select(fp => new FiscalPeriodLookupItem
                    {
                        CompanyMonthId = fp.CompanyMonthId,
                        CompanyMonthName = fp.CompanyMonthName!
                    })
                    .ToListAsync();

                return fiscalPeriods;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<FiscalPeriodLookupItem>>(
                    new Error("LookupRepository.RetrieveFiscalPeriodsAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<VoucherTypeLookupItem>>> RetrieveVoucherTypesAsync()
        {
            try
            {
                List<VoucherTypeLookupItem> voucherTypes = await context.Vouchers
                    .OrderBy(vt => vt.VoucherCode)
                    .Select(vt => new VoucherTypeLookupItem
                    {
                        VoucherCode = vt.VoucherCode,
                        VoucherType = vt.VoucherType
                    })
                    .ToListAsync();

                return voucherTypes;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<VoucherTypeLookupItem>>(
                    new Error("LookupRepository.RetrieveCVoucherTypesAsync", errMsg)
                );
            }
        }

        public async Task<Result<List<CoaLookupItem>>> RetrieveLedgerAccountsAsync(int companyCode)
        {
            try
            {
                List<CoaLookupItem> coaItems = await context.ChartOfAccounts
                    .Where(coa => coa.CompanyCode == companyCode && coa.AccountLevel == 4)
                    .OrderBy(coa => coa.AccountCode)
                    .Select(coa => new CoaLookupItem
                    {
                        AccountCode = coa.AccountCode,
                        AccountTitle = coa.AccountTitle
                    })
                    .ToListAsync();

                return coaItems;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure<List<CoaLookupItem>>(
                    new Error("LookupRepository.RetrieveLedgerAccountsAsync", errMsg)
                );
            }
        }
    }
}