namespace CloudAccounting.Core.Services;


public class CompanyService(ICompanyRepository repository)
{
    private readonly ICompanyRepository _repository = repository;

    public async Task<Result<FiscalYear>> AddFiscalYear(Company company, int fiscalYearNumber, int startMonthNumber)
    {
        DateTime startDate = new(fiscalYearNumber, startMonthNumber, 1);
        DateTime oneYearLater = startDate.AddMonths(11);
        int daysInMonth = DateTime.DaysInMonth(startDate.AddMonths(11).Year, startDate.AddMonths(11).Month);
        DateTime lastDateInFiscalYear = new(oneYearLater.Year, oneYearLater.Month, daysInMonth);

        FiscalYear fiscalYear = new
        (
            fiscalYearNumber,
            startDate,
            lastDateInFiscalYear,
            true,
            false,
            false,
            DateTime.MaxValue,
            []
        );

        fiscalYear.CreateFiscalPeriods();

        return fiscalYear;
    }
}
