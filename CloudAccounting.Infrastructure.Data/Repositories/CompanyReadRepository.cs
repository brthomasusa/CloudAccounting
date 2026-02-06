using CloudAccounting.Infrastructure.Data.Interfaces;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class CompanyReadRepository
    (
        DapperContext dapperContext,
        ILogger<CompanyReadRepository> logger
    ) : ICompanyReadRepository
    {
        private readonly DapperContext _dapperContext = dapperContext;
        private ILogger<CompanyReadRepository> _logger = logger;

        public Task<Result<CompanyDto>> GetCompanyDtoByIdAsync(int companyCode)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<CompanyWithFiscalPeriodsDto>> GetFiscalYearDtoByIdAndYearAsync(int companyCode, int fiscalYearNumber)
        {
            try
            {
                string sql =
                @"SELECT
                    c.cocode as CompanyCode, 
                    c.coname as CompanyName,
                    fy.coyear as FiscalYear,
                    fy.initial_year as IsInitialYear,
                    fy.comonthid as MonthId,
                    fy.comonthname as MonthName,
                    fy.pfrom as StartDate,
                    fy.pto as EndDate,                    
                    fy.year_closed as IsYearClosed,
                    fy.month_closed as IsPeriodClose,
                    fy.tye_executed as TempYrEndExecuted
                FROM gl_company c JOIN gl_fiscal_year fy on c.cocode = fy.cocode
                WHERE c.cocode = :COCODE and fy.coyear = :COYEAR                  
                ORDER BY fy.comonthid";


                using var connection = _dapperContext.CreateConnection();
                {
                    var companyDictionary = new Dictionary<int, CompanyWithFiscalPeriodsDto>();

                    var list = connection.Query<CompanyWithFiscalPeriodsDto, FiscalPeriodDto, CompanyWithFiscalPeriodsDto>(
                        sql,
                        (company, fiscalPeriods) =>
                        {
                            CompanyWithFiscalPeriodsDto companyDto;

                            if (!companyDictionary.TryGetValue(company.CompanyCode, out companyDto!))
                            {
                                companyDto = company;
                                companyDto.FiscalPeriods = [];
                                companyDictionary.Add(companyDto.CompanyCode, companyDto);
                            }

                            companyDto.FiscalPeriods.Add(fiscalPeriods);
                            return companyDto;
                        },
                        param: new { COCODE = companyCode, COYEAR = fiscalYearNumber },
                        splitOn: "MonthId")
                    .Distinct()
                    .ToList();

                    CompanyWithFiscalPeriodsDto? companyDto = list.FirstOrDefault();

                    if (companyDto is null)
                    {
                        string errMsg = "Unknown error! Failed to retrieve company with fiscal periods info.";
                        _logger.LogWarning("{Message}", errMsg);

                        return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                            new Error("CompanyReadRepository.GetFiscalYearDtoByIdAndYearAsync", errMsg)
                        );
                    }

                    return companyDto;
                }

            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                    new Error("CompanyReadRepository.GetFiscalYearDtoByIdAndYearAsync", errMsg)
                );
            }
        }

        public async Task<Result<CompanyWithFiscalPeriodsDto>> GetLatestFiscalYearForCompanyAsync(int companyCode)
        {
            try
            {
                string sql =
                @"SELECT
                    c.cocode as CompanyCode, 
                    c.coname as CompanyName,
                    fy.coyear as FiscalYear,
                    fy.initial_year as IsInitialYear,
                    fy.comonthid as MonthId,
                    fy.comonthname as MonthName,
                    fy.pfrom as StartDate,
                    fy.pto as EndDate,                    
                    fy.year_closed as IsYearClosed,
                    fy.month_closed as IsPeriodClose,
                    fy.tye_executed as TempYrEndExecuted
                FROM
                    gl_company c JOIN gl_fiscal_year fy on c.cocode = fy.cocode 
                WHERE
                    fy.coyear IN (SELECT DISTINCT COYEAR FROM gl_fiscal_year WHERE COCODE = :COCODE ORDER BY coyear desc FETCH FIRST 1 ROWS ONLY)
                    AND c.cocode = :COCODE
                ORDER BY fy.comonthid";

                using var connection = _dapperContext.CreateConnection();
                {
                    var companyDictionary = new Dictionary<int, CompanyWithFiscalPeriodsDto>();

                    var list = connection.Query<CompanyWithFiscalPeriodsDto, FiscalPeriodDto, CompanyWithFiscalPeriodsDto>(
                        sql,
                        (company, fiscalPeriods) =>
                        {
                            CompanyWithFiscalPeriodsDto companyDto;

                            if (!companyDictionary.TryGetValue(company.CompanyCode, out companyDto!))
                            {
                                companyDto = company;
                                companyDto.FiscalPeriods = [];
                                companyDictionary.Add(companyDto.CompanyCode, companyDto);
                            }

                            companyDto.FiscalPeriods.Add(fiscalPeriods);
                            return companyDto;
                        },
                        param: new { COCODE = companyCode },
                        splitOn: "MonthId")
                    .Distinct()
                    .ToList();

                    CompanyWithFiscalPeriodsDto? companyDto = list.FirstOrDefault();

                    if (companyDto is null)
                    {
                        string companyName = string.Empty;

                        Result<string> result = await GetCompanyName(companyCode);
                        if (result.IsSuccess)
                        {
                            companyName = result.Value;
                        }

                        companyDto = new()
                        {
                            CompanyCode = companyCode,
                            CompanyName = companyName,
                            FiscalYear = 0,
                            IsInitialYear = false
                        };

                        return companyDto;
                    }

                    return companyDto;
                }
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                    new Error("CompanyReadRepository.GetLatestFiscalYearForCompanyAsync", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsUniqueCompanyNameForCreate(string companyName)
        {
            try
            {
                string sql = "SELECT is_unique_companyname_for_create(:CONAME) AS FROM DUAL";

                var param = new { CONAME = companyName };

                using var connection = _dapperContext.CreateConnection();

                int retval = await connection.ExecuteScalarAsync<int>(sql, param);

                // 0 means the name is not unique; 1 means it is unique
                return retval == 1;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyRepository.IsUniqueCompanyName", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsUniqueCompanyNameForUpdate(int companyCode, string companyName)
        {
            try
            {
                string sql = "SELECT is_unique_companyname_for_update(:COCODE,:CONAME) FROM DUAL";

                var param = new { COCODE = companyCode, CONAME = companyName };

                using var connection = _dapperContext.CreateConnection();

                int retval = await connection.ExecuteScalarAsync<int>(sql, param);

                return retval == 1;
            }
            catch (OracleException ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyRepository.IsExistingCompany", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsExistingCompany(int companyCode)
        {
            try
            {
                string sql = "SELECT is_existing_company(:COCODE) AS CompanyExists FROM DUAL";

                var param = new { COCODE = companyCode };

                using var connection = _dapperContext.CreateConnection();

                int retval = await connection.ExecuteScalarAsync<int>(sql, param);

                return retval == 1;
            }
            catch (OracleException ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyRepository.IsExistingCompany", errMsg)
                );
            }
        }

        public Task<Result<bool>> CanCompanyBeDeleted(int companyCode)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> CanCompanyFiscalYearBeDeleted(int companyCode, int yearNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> InitialFiscalYearExist(int companyCode)
        {
            try
            {
                string sql = "SELECT is_initial_fiscalyear(:COCODE) AS FiscalYearExists FROM DUAL";

                var param = new { COCODE = companyCode };

                using var connection = _dapperContext.CreateConnection();

                int retval = await connection.ExecuteScalarAsync<int>(sql, param);

                return retval == 1;
            }
            catch (OracleException ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("CompanyReadRepository.InitialFiscalYearExist", errMsg)
                );
            }
        }

        public async Task<Result<string>> GetCompanyName(int companyCode)
        {
            try
            {
                string sql = "SELECT get_company_name(:COCODE) AS FiscalYearExists FROM DUAL";

                var param = new { COCODE = companyCode };

                using var connection = _dapperContext.CreateConnection();

                string? companyName = await connection.ExecuteScalarAsync<string>(sql, param);

                return companyName!;
            }
            catch (OracleException ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<string>.Failure<string>(
                    new Error("CompanyReadRepository.GetCompanyName", errMsg)
                );
            }
        }

        public async Task<Result<List<CompanyLookup>>> GetCompanyLookups()
        {
            try
            {
                string sql =
                @"SELECT 
                    COCODE AS CompanyCode, 
                    CONAME AS CompanyName 
                FROM gl_company 
                ORDER BY CONAME";

                using var connection = _dapperContext.CreateConnection();

                var lookups = await connection.QueryAsync<CompanyLookup>(sql);

                return lookups.ToList();
            }
            catch (OracleException ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<CompanyLookup>>.Failure<List<CompanyLookup>>(
                    new Error("CompanyReadRepository.GetCompanyLookups(", errMsg)
                );
            }
        }

        public async Task<Result<DateTime>> GetNextValidFiscalYearStartDate(int companyCode)
        {
            try
            {
                string sql = "SELECT get_next_valid_fiscalyear_startdate(:COCODE) FROM DUAL";

                var param = new { COCODE = companyCode };

                using var connection = _dapperContext.CreateConnection();

                DateTime nextValidDate = await connection.ExecuteScalarAsync<DateTime>(sql, param);

                // 0 means the name is not unique; 1 means it is unique
                return nextValidDate;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<DateTime>.Failure<DateTime>(
                    new Error("CompanyRepository.GetNextValidFiscalYearStartDate", errMsg)
                );
            }
        }
    }
}