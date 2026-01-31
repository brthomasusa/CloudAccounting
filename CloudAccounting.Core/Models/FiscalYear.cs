namespace CloudAccounting.Core.Models;

public record FiscalYear
(
    int Year,
    DateTime FiscalYearStartDate,
    DateTime FiscalYearEndDate,
    bool IsInitialFiscalYear,
    bool IsFiscalYearClosed,
    bool HasTransactions,
    DateTime TemporaryYearEndProcessLastExecuted,
    List<FiscalPeriod> FiscalPeriods
)
{
    public void CreateFiscalPeriods()
    {
        int monthNumber = this.FiscalYearStartDate.Month;
        int yearNumber = this.Year;
        int changeYearAndMonth = ChangeYearAndMonth(this.FiscalYearStartDate.Month);

        for (int count = 1; count < 13; count++)
        {
            if (count == changeYearAndMonth)
            {
                yearNumber++;
                monthNumber = 1;
            }

            this.FiscalPeriods.Add(GetFiscalPeriod(yearNumber, monthNumber)!);
            monthNumber++;
        }
    }

    private static FiscalPeriod? GetFiscalPeriod(int year, int monthNumber)
    {
        int lastDayOfFebruary = DateTime.IsLeapYear(year) ? 29 : 28;

        return monthNumber switch
        {
            1 => new FiscalPeriod(monthNumber, "January", new DateTime(year, 1, 1), new DateTime(year, 1, 31), false),
            2 => new FiscalPeriod(monthNumber, "February", new DateTime(year, 2, 1), new DateTime(year, 2, lastDayOfFebruary), false),
            3 => new FiscalPeriod(monthNumber, "March", new DateTime(year, 3, 1), new DateTime(year, 3, 31), false),
            4 => new FiscalPeriod(monthNumber, "April", new DateTime(year, 4, 1), new DateTime(year, 4, 30), false),
            5 => new FiscalPeriod(monthNumber, "May", new DateTime(year, 5, 1), new DateTime(year, 5, 31), false),
            6 => new FiscalPeriod(monthNumber, "June", new DateTime(year, 6, 1), new DateTime(year, 6, 30), false),
            7 => new FiscalPeriod(monthNumber, "July", new DateTime(year, 7, 1), new DateTime(year, 7, 31), false),
            8 => new FiscalPeriod(monthNumber, "August", new DateTime(year, 8, 1), new DateTime(year, 8, 31), false),
            9 => new FiscalPeriod(monthNumber, "September", new DateTime(year, 9, 1), new DateTime(year, 9, 30), false),
            10 => new FiscalPeriod(monthNumber, "October", new DateTime(year, 10, 1), new DateTime(year, 10, 31), false),
            11 => new FiscalPeriod(monthNumber, "November", new DateTime(year, 11, 1), new DateTime(year, 11, 30), false),
            12 => new FiscalPeriod(monthNumber, "December", new DateTime(year, 12, 1), new DateTime(year, 12, 31), false),
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
}
