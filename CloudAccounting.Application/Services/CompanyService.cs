using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CloudAccounting.Application.Services
{
    public class CompanyService(ICompanyRepository repository, ICompanyReadRepository readRepository) : ICompanyService
    {
        private readonly ICompanyRepository _repository = repository;
        private readonly ICompanyReadRepository _readRepository = readRepository;


        public async Task<Result<FiscalYear>> CreateFiscalYearWithPeriods(int companyCode, int fiscalYearNumber, int startMonthNumber)
        {
            DateTime startDate = new(fiscalYearNumber, startMonthNumber, 1);
            DateTime oneYearLater = startDate.AddMonths(11);
            int daysInMonth = DateTime.DaysInMonth(startDate.AddMonths(11).Year, startDate.AddMonths(11).Month);
            DateTime lastDateInFiscalYear = new(oneYearLater.Year, oneYearLater.Month, daysInMonth);

            FiscalYear fiscalYear = new
            (
                companyCode,
                fiscalYearNumber,
                startDate,
                lastDateInFiscalYear,
                await IsInitialFiscalYear(companyCode),
                false,
                false,
                DateTime.MinValue,
                [],
                await GetCompanyName(companyCode)
            );

            CreateFiscalPeriods(fiscalYear);

            return fiscalYear;
        }

        internal static void CreateFiscalPeriods(FiscalYear fiscalYear)
        {
            int monthNumber = fiscalYear.FiscalYearStartDate.Month;
            int yearNumber = fiscalYear.Year;
            int changeYearAndMonth = ChangeYearAndMonth(fiscalYear.FiscalYearStartDate.Month);

            for (int count = 1; count < 13; count++)
            {
                if (count == changeYearAndMonth)
                {
                    yearNumber++;
                    monthNumber = 1;
                }

                fiscalYear.FiscalPeriods.Add(GetFiscalPeriod(count, yearNumber, monthNumber)!);
                monthNumber++;
            }
        }

        private static FiscalPeriod? GetFiscalPeriod(int monthId, int year, int monthNumber)
        {
            int lastDayOfFebruary = DateTime.IsLeapYear(year) ? 29 : 28;

            return monthNumber switch
            {
                1 => new FiscalPeriod(monthId, "January", new DateTime(year, 1, 1), new DateTime(year, 1, 31), false),
                2 => new FiscalPeriod(monthId, "February", new DateTime(year, 2, 1), new DateTime(year, 2, lastDayOfFebruary), false),
                3 => new FiscalPeriod(monthId, "March", new DateTime(year, 3, 1), new DateTime(year, 3, 31), false),
                4 => new FiscalPeriod(monthId, "April", new DateTime(year, 4, 1), new DateTime(year, 4, 30), false),
                5 => new FiscalPeriod(monthId, "May", new DateTime(year, 5, 1), new DateTime(year, 5, 31), false),
                6 => new FiscalPeriod(monthId, "June", new DateTime(year, 6, 1), new DateTime(year, 6, 30), false),
                7 => new FiscalPeriod(monthId, "July", new DateTime(year, 7, 1), new DateTime(year, 7, 31), false),
                8 => new FiscalPeriod(monthId, "August", new DateTime(year, 8, 1), new DateTime(year, 8, 31), false),
                9 => new FiscalPeriod(monthId, "September", new DateTime(year, 9, 1), new DateTime(year, 9, 30), false),
                10 => new FiscalPeriod(monthId, "October", new DateTime(year, 10, 1), new DateTime(year, 10, 31), false),
                11 => new FiscalPeriod(monthId, "November", new DateTime(year, 11, 1), new DateTime(year, 11, 30), false),
                12 => new FiscalPeriod(monthId, "December", new DateTime(year, 12, 1), new DateTime(year, 12, 31), false),
                _ => null
            };
        }

        private static int ChangeYearAndMonth(int startMonth)
            => startMonth switch
            {
                1 => 0,
                2 => 12,
                3 => 11,
                4 => 10,
                5 => 9,
                6 => 8,
                7 => 7,
                8 => 6,
                9 => 5,
                10 => 4,
                11 => 3,
                12 => 2,
                _ => -1
            };

        private async Task<string> GetCompanyName(int companyCode)
        {
            Result<string> result = await _readRepository.GetCompanyName(companyCode);

            if (result.IsSuccess)
            {
                return result.Value!;
            }
            return string.Empty;
        }

        private async Task<bool> IsInitialFiscalYear(int companyCode)
        {
            Result<bool> result = await _readRepository.InitialFiscalYearExist(companyCode);

            return !result.Value!;
        }
    }
}