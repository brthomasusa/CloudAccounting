using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class FiscalYearRepository
    (
        CloudAccountingContext ctx,
        ILogger<FiscalYearRepository> logger
    ) : IFiscalYearRepository
    {
        private readonly CloudAccountingContext _db = ctx;
        private readonly ILogger<FiscalYearRepository> _logger = logger;

        public async Task<Result<FiscalYear>> GetFiscalYearAsync(int companyCode, int fiscalYearNumber)
        {
            try
            {
                Result<string> companyNameResult = await GetCompanyName(companyCode);
                string companyName = companyNameResult.IsSuccess ? companyNameResult.Value : "Unknown Company";

                int transactionCount = await _db.TranMasters
                                                .CountAsync(t => t.CompanyCode == companyCode && t.CompanyYear == fiscalYearNumber);

                List<FiscalYearDM>? fiscalPeriods =
                    await _db.FiscalYears
                             .Include(c => c.CompanyCodeNavigation)
                             .Where(fy => fy.CompanyCode == companyCode && fy.CompanyYear == fiscalYearNumber)
                             .OrderBy(fy => fy.CompanyMonthId)
                             .ToListAsync();

                List<FiscalPeriod> periods = [];
                fiscalPeriods.ForEach(p =>
                    periods.Add(new FiscalPeriod(p.CompanyMonthId, p.CompanyMonthName!, p.PeriodFrom!.Value, p.PeriodTo!.Value, p.MonthClosed!.Value))
                );

                if (fiscalPeriods is null || fiscalPeriods.Count == 0)
                {
                    return new FiscalYear(
                        companyCode,
                        companyName,
                        fiscalYearNumber,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        false,
                        false,
                        false,
                        null,
                        []
                    );
                }

                var firstPeriod = fiscalPeriods.SingleOrDefault(fy => fy.CompanyMonthId == 1);
                var lastPeriod = fiscalPeriods.SingleOrDefault(fy => fy.CompanyMonthId == 12);

                FiscalYear fiscalYear = new(
                    companyCode,
                    firstPeriod!.CompanyCodeNavigation.CompanyName!,
                    fiscalYearNumber,
                    firstPeriod!.PeriodFrom!.Value,
                    lastPeriod!.PeriodTo!.Value,
                    firstPeriod!.InitialYear!.Value,
                    firstPeriod!.YearClosed!.Value,
                    transactionCount > 0,
                    firstPeriod!.TyeExecuted,
                    periods
                );

                return fiscalYear;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<FiscalYear>.Failure<FiscalYear>(
                    new Error("FiscalYearRepository.GetFiscalYearByCompanyAndYearAsync", errMsg)
                );
            }
        }

        public async Task<Result<FiscalYear>> GetMostRecentFiscalYearAsync(int companyCode)
        {
            try
            {
                int? mostRecentFiscalYearNumber = await _db.FiscalYears
                                                           .Where(fy => fy.CompanyCode == companyCode)
                                                           .MaxAsync(fy => (int?)fy.CompanyYear);

                if (mostRecentFiscalYearNumber is null)
                {
                    Result<string> companyNameResult = await GetCompanyName(companyCode);

                    return new FiscalYear(
                        companyCode,
                        companyNameResult.IsSuccess ? companyNameResult.Value : "Unknown Company",
                        0,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        false,
                        false,
                        false,
                        null,
                        []
                    );
                }

                return await GetFiscalYearAsync(companyCode, mostRecentFiscalYearNumber.Value);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<FiscalYear>.Failure<FiscalYear>(
                    new Error("FiscalYearRepository.GetMostRecentFiscalYearAsync", errMsg)
                );
            }
        }

        public async Task<Result<FiscalYear>> InsertFiscalYearAsync(FiscalYear fiscalYear)
        {
            try
            {
                bool doesInitYearExist =
                    await _db.FiscalYears.AnyAsync(f => f.CompanyCode == fiscalYear.CompanyCode && f.InitialYear == true);

                List<FiscalYearDM> dataModels =
                    MapFiscalYearToFiscalYearDMList(fiscalYear, !doesInitYearExist);

                _db.FiscalYears.AddRange(dataModels);
                await _db.SaveChangesAsync();

                return await GetFiscalYearAsync(fiscalYear.CompanyCode, fiscalYear.Year);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<FiscalYear>.Failure<FiscalYear>(
                    new Error("FiscalYearRepository.InsertFiscalYearAsync", errMsg)
                );
            }
        }

        public async Task<Result> DeleteFiscalYearAsync(int companyCode, int fiscalYear)
        {
            try
            {
                await _db.FiscalYears.Where(c => c.CompanyCode == companyCode && c.CompanyYear == fiscalYear)
                                     .ExecuteDeleteAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure(new Error("FiscalYearRepository.DeleteFiscalYearAsync", errMsg));
            }
        }

        public async Task<Result<bool>> CanFiscalYearBeDeleted(int companyCode, int fiscalYearNumber)
        {
            try
            {
                bool doesFiscalYearHaveTransactions =
                    await _db.TranMasters.AnyAsync(t => t.CompanyCode == companyCode && t.CompanyYear == fiscalYearNumber);

                return doesFiscalYearHaveTransactions == false;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("FiscalYearRepository.CanFiscalYearBeDeleted", errMsg)
                );
            }
        }

        public async Task<Result<DateTime>> EarliestNextFiscalYearStartDate(int companyCode)
        {
            try
            {
                var query = await _db.FiscalYears
                                     .Where(fy => fy.CompanyCode == companyCode)
                                     .OrderByDescending(fy => fy.PeriodTo)
                                     .Take(1)
                                     .ToListAsync();

                if (query is not null && query.Count > 0)
                {
                    FiscalYearDM lastPeriod = query[0];
                    return lastPeriod.PeriodTo!.Value.AddDays(1);
                }

                return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<DateTime>.Failure<DateTime>(
                    new Error("FiscalYearRepository.EarliestNextFiscalYearStartDate", errMsg)
                );
            }
        }

        public async Task<Result<string>> GetCompanyName(int companyCode)
        {
            try
            {
                string? companyName = await _db.Companies
                                               .Where(c => c.CompanyCode == companyCode)
                                               .Select(c => c.CompanyName)
                                               .SingleOrDefaultAsync();
                if (companyName is null)
                {
                    return Result<string>.Failure<string>(
                        new Error("FiscalYearRepository.GetCompanyName", "There was a problem retrieving the company name!"));
                }

                return companyName;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<string>.Failure<string>(
                    new Error("FiscalYearRepository.GetCompanyName", errMsg)
                );
            }
        }

        public async Task<Result<bool>> DoesCompanyHaveInitialFiscalYear(int companyCode)
        {
            try
            {
                var query = await _db.FiscalYears
                                     .Where(c => c.CompanyCode == companyCode && c.InitialYear == true)
                                     .Select(c => c.CompanyYear)
                                     .ToListAsync();

                return query is not null && query.Count > 0;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("FiscalYearRepository.DoesCompanyHaveIsInitialFiscalYear", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsValidCompanyCode(int companyCode)
        {
            try
            {
                var company = await _db.Companies.SingleOrDefaultAsync(c => c.CompanyCode == companyCode);

                return company is not null;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("FiscalYearRepository.IsValidCompanyCode", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsUniqueFiscalYearNumber(int companyCode, int fiscalYearNumber)
        {
            try
            {
                bool doesExist = await _db.FiscalYears
                                          .AnyAsync(fy => fy.CompanyCode == companyCode && fy.CompanyYear == fiscalYearNumber);

                return !doesExist;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("FiscalYearRepository.IsUniqueFiscalYearNumber", errMsg)
                );
            }
        }

        private static List<FiscalYearDM> MapFiscalYearToFiscalYearDMList
        (
            FiscalYear fiscalYear,
            bool isInitYr
        )
        {
            List<FiscalYearDM> dataModels = [];

            fiscalYear.FiscalPeriods.ForEach(p =>
            {
                dataModels.Add(new FiscalYearDM()
                {
                    CompanyCode = fiscalYear.CompanyCode,
                    CompanyYear = (short)fiscalYear.Year,
                    CompanyMonthId = (byte)p.MonthId,
                    CompanyMonthName = p.MonthName,
                    PeriodFrom = p.StartDate,
                    PeriodTo = p.EndDate,
                    InitialYear = isInitYr,
                    YearClosed = false,
                    MonthClosed = false
                });
            });

            return dataModels;
        }
    }
}