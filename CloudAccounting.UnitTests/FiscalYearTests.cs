using CloudAccounting.Application.Models;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Oracle.EntityFrameworkCore.Query.Internal;

namespace CloudAccounting.UnitTests;

public class FiscalYearTests
{
    [Fact]
    public void Create_FiscalYear_Start_Jan()
    {
        // Arrange
        FiscalYearDto fiscalYear = new
        (
            2024,
            new DateTime(2024, 1, 1),
            new DateTime(2024, 12, 31),
            true,
            false,
            false,
            DateTime.MaxValue,
            []
        );

        int monthNumber = fiscalYear.FiscalYearStartDate.Month;
        int yearNumber = fiscalYear.FiscalYear;
        int changeYearAndMonth = ChangeYearAndMonth(fiscalYear.FiscalYearStartDate.Month);

        // Act
        for (int count = 1; count < 13; count++)
        {
            if (count == changeYearAndMonth)
            {
                yearNumber++;
                monthNumber = 1;
            }

            fiscalYear.FiscalPeriods.Add(GetFiscalPeriod(yearNumber, monthNumber)!);
            monthNumber++;
        }

        // Assert
        FiscalPeriodDto? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriodDto>();
        FiscalPeriodDto? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriodDto>();

        Assert.Equal(12, fiscalYear.FiscalPeriods.Count);
        Assert.Equal(new DateTime(2024, 1, 1), firstPeriod!.StartDate);
        Assert.Equal(new DateTime(2024, 1, 31), firstPeriod!.EndDate);
        Assert.Equal(new DateTime(2024, 12, 1), lastPeriod!.StartDate);
        Assert.Equal(new DateTime(2024, 12, 31), lastPeriod!.EndDate);
    }

    [Fact]
    public void Create_FiscalYear_Start_Feb()
    {
        // Arrange
        int lastDayOfFebruary = DateTime.IsLeapYear(2024) ? 29 : 28;

        FiscalYearDto fiscalYear = new
        (
            2024,
            new DateTime(2024, 2, 1),
            new DateTime(2025, 1, 31),
            true,
            false,
            false,
            DateTime.MaxValue,
            []
        );

        int monthNumber = fiscalYear.FiscalYearStartDate.Month;
        int yearNumber = fiscalYear.FiscalYear;
        int changeYearAndMonth = ChangeYearAndMonth(fiscalYear.FiscalYearStartDate.Month);

        // Act
        for (int count = 1; count < 13; count++)
        {
            if (count == changeYearAndMonth)
            {
                yearNumber++;
                monthNumber = 1;
            }

            fiscalYear.FiscalPeriods.Add(GetFiscalPeriod(yearNumber, monthNumber)!);
            monthNumber++;
        }

        // Assert
        FiscalPeriodDto? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriodDto>();
        FiscalPeriodDto? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriodDto>();

        Assert.Equal(12, fiscalYear.FiscalPeriods.Count);
        Assert.Equal(new DateTime(2024, 2, 1), firstPeriod!.StartDate);
        Assert.Equal(new DateTime(2024, 2, lastDayOfFebruary), firstPeriod!.EndDate);
        Assert.Equal(new DateTime(2025, 1, 1), lastPeriod!.StartDate);
        Assert.Equal(new DateTime(2025, 1, 31), lastPeriod!.EndDate);
    }

    [Fact]
    public void Create_FiscalYear_Start_Mar()
    {
        // Arrange
        int lastDayOfFebruary = DateTime.IsLeapYear(2025) ? 29 : 28;

        FiscalYearDto fiscalYear = new
        (
            2024,
            new DateTime(2024, 3, 1),
            new DateTime(2025, 2, lastDayOfFebruary),
            true,
            false,
            false,
            DateTime.MaxValue,
            []
        );

        int monthNumber = fiscalYear.FiscalYearStartDate.Month;
        int yearNumber = fiscalYear.FiscalYear;
        int changeYearAndMonth = ChangeYearAndMonth(fiscalYear.FiscalYearStartDate.Month);

        // Act
        for (int count = 1; count < 13; count++)
        {
            if (count == changeYearAndMonth)
            {
                yearNumber++;
                monthNumber = 1;
            }

            fiscalYear.FiscalPeriods.Add(GetFiscalPeriod(yearNumber, monthNumber)!);
            monthNumber++;
        }

        // Assert
        FiscalPeriodDto? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriodDto>();
        FiscalPeriodDto? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriodDto>();

        Assert.Equal(12, fiscalYear.FiscalPeriods.Count);
        Assert.Equal(new DateTime(2024, 3, 1), firstPeriod!.StartDate);
        Assert.Equal(new DateTime(2024, 3, 31), firstPeriod!.EndDate);
        Assert.Equal(new DateTime(2025, 2, 1), lastPeriod!.StartDate);
        Assert.Equal(new DateTime(2025, 2, lastDayOfFebruary), lastPeriod!.EndDate);
    }

    private static FiscalPeriodDto? GetFiscalPeriod(int year, int monthNumber)
    {
        int lastDayOfFebruary = DateTime.IsLeapYear(year) ? 29 : 28;

        return monthNumber switch
        {
            1 => new FiscalPeriodDto(monthNumber, "January", new DateTime(year, 1, 1), new DateTime(year, 1, 31), false),
            2 => new FiscalPeriodDto(monthNumber, "February", new DateTime(year, 2, 1), new DateTime(year, 2, lastDayOfFebruary), false),
            3 => new FiscalPeriodDto(monthNumber, "March", new DateTime(year, 3, 1), new DateTime(year, 3, 31), false),
            4 => new FiscalPeriodDto(monthNumber, "April", new DateTime(year, 4, 1), new DateTime(year, 4, 30), false),
            5 => new FiscalPeriodDto(monthNumber, "May", new DateTime(year, 5, 1), new DateTime(year, 5, 31), false),
            6 => new FiscalPeriodDto(monthNumber, "June", new DateTime(year, 6, 1), new DateTime(year, 6, 30), false),
            7 => new FiscalPeriodDto(monthNumber, "July", new DateTime(year, 7, 1), new DateTime(year, 7, 31), false),
            8 => new FiscalPeriodDto(monthNumber, "August", new DateTime(year, 8, 1), new DateTime(year, 8, 31), false),
            9 => new FiscalPeriodDto(monthNumber, "September", new DateTime(year, 9, 1), new DateTime(year, 9, 30), false),
            10 => new FiscalPeriodDto(monthNumber, "October", new DateTime(year, 10, 1), new DateTime(year, 10, 31), false),
            11 => new FiscalPeriodDto(monthNumber, "November", new DateTime(year, 11, 1), new DateTime(year, 11, 30), false),
            12 => new FiscalPeriodDto(monthNumber, "December", new DateTime(year, 12, 1), new DateTime(year, 12, 31), false),
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

    private void TestData()
    {
        /*
            int FiscalYear,
            DateTime FiscalYearStartDate,
            DateTime FiscalYearEndDate,
            bool IsInitialFiscalYear,
            bool IsFiscalYearClosed,
            bool HasTransactions,
            DateTime TemporaryYearEndProcessLastExecuted,
            List<FiscalPeriodDto> FiscalPeriods
        */
    }
}
