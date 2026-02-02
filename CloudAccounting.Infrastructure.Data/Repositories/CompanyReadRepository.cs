using CloudAccounting.Infrastructure.Data;
using CloudAccounting.Infrastructure.Data.Interfaces;
using CloudAccounting.Shared.Company;
using Dapper;

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
                    fy.comonthid as MonthNumber,
                    fy.comonthname as MonthName,
                    fy.pfrom as PeriodStart,
                    fy.pto as PeriodEnd,                    
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
                        splitOn: "MonthNumber")
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
    }
}