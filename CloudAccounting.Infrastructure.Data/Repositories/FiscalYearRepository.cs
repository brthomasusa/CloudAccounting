using Dapper;
using Microsoft.EntityFrameworkCore;                    // To use ToArrayAsync.
using Microsoft.Extensions.Caching.Memory;
using CloudAccounting.Core.Models;
using FiscalYearDataModel = CloudAccounting.Infrastructure.Data.Models.FiscalYear;
using FiscalYearDomainModel = CloudAccounting.Core.Models.FiscalYear;


namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class FiscalYearRepository
    (
        CloudAccountingContext ctx,
        IMemoryCache memoryCache,
        ILogger<FiscalYearRepository> logger,
        DapperContext oracleContext,
        IMapper mapper
    ) : IFiscalYearRepository
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

        private readonly CloudAccountingContext _db = ctx;
        private readonly ILogger<FiscalYearRepository> _logger = logger;
        private readonly DapperContext _oracleContext = oracleContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<FiscalYearDomainModel>> GetFiscalYearByCompanyAndYearAsync(int companyCode, int fiscalYearNumber)
        {
            try
            {
                List<FiscalYearDataModel>? dataModels =
                    await _db.FiscalYears.Where(fy => fy.CompanyCode == companyCode && fy.CompanyYear == fiscalYearNumber)
                                         .OrderBy(fy => fy.CompanyMonthId)
                                         .ToListAsync();

                return MapFiscalYearDataModelListToFiscalYearDomainModel(dataModels);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<FiscalYearDomainModel>.Failure<FiscalYearDomainModel>(
                    new Error("FiscalYearRepository.GetFiscalYearByCompanyAndYearAsync", errMsg)
                );
            }
        }

        public async Task<Result<FiscalYearDomainModel>> InsertFiscalYearAsync(FiscalYearDomainModel fiscalYear)
        {
            try
            {
                FiscalYearDataModel? initYearTest =
                    await _db.FiscalYears.FirstOrDefaultAsync(f => f.CompanyCode == fiscalYear.CompanyCode && f.InitialYear == true);

                bool isInitialYear = initYearTest is null;

                List<FiscalYearDataModel> dataModels =
                    MapFiscalYearDomainModelToFiscalYearDataModelList(fiscalYear, isInitialYear);

                _db.FiscalYears.AddRange(dataModels);
                await _db.SaveChangesAsync();


                return await GetFiscalYearByCompanyAndYearAsync(fiscalYear.CompanyCode, fiscalYear.Year);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<FiscalYearDomainModel>.Failure<FiscalYearDomainModel>(
                    new Error("FiscalYearRepository.InsertFiscalYearAsync", errMsg)
                );
            }
        }

        private static FiscalYearDomainModel MapFiscalYearDataModelListToFiscalYearDomainModel(List<FiscalYearDataModel> fiscalYears)
        {
            var firstRow = fiscalYears.FirstOrDefault(fy => fy.CompanyMonthId == 1);
            var lastRow = fiscalYears.FirstOrDefault(fy => fy.CompanyMonthId == 12);

            FiscalYearDomainModel domainModel =
                new(
                    firstRow!.CompanyCode,
                    firstRow!.CompanyYear,
                    firstRow!.PeriodFrom!.Value,
                    lastRow!.PeriodTo!.Value,
                    firstRow!.InitialYear!.Value,
                    firstRow!.YearClosed!.Value,
                    false,
                    firstRow!.TyeExecuted!.HasValue ? firstRow!.TyeExecuted!.Value : null,
                    []
                );

            fiscalYears.ForEach(fy =>
            {
                domainModel.FiscalPeriods.Add(new FiscalPeriod(
                    fy.CompanyMonthId,
                    fy.CompanyMonthName!,
                    fy.PeriodFrom!.Value,
                    fy.PeriodTo!.Value,
                    fy.MonthClosed!.Value
                ));
            });

            return domainModel;
        }

        private static List<FiscalYearDataModel> MapFiscalYearDomainModelToFiscalYearDataModelList
        (
            FiscalYearDomainModel fiscalYear,
            bool isInitYr
        )
        {
            List<FiscalYearDataModel> dataModels = [];

            fiscalYear.FiscalPeriods.ForEach(p =>
            {
                dataModels.Add(new FiscalYearDataModel()
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