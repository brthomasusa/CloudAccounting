// using CloudAccounting.Application.Models;
using CloudAccounting.Core.Models;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Oracle.EntityFrameworkCore.Query.Internal;

namespace CloudAccounting.UnitTests;

public class FiscalYearTests
{
    [Fact]
    public void Create_FiscalYear_Start_Jan()
    {
        // Arrange
        FiscalYear fiscalYear = new
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

        // Act
        fiscalYear.CreateFiscalPeriods();

        // Assert
        FiscalPeriod? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriod>();
        FiscalPeriod? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriod>();

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

        FiscalYear fiscalYear = new
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

        // Act
        fiscalYear.CreateFiscalPeriods();

        // Assert
        FiscalPeriod? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriod>();
        FiscalPeriod? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriod>();

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

        FiscalYear fiscalYear = new
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

        // Act
        fiscalYear.CreateFiscalPeriods();

        // Assert
        FiscalPeriod? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriod>();
        FiscalPeriod? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriod>();

        Assert.Equal(12, fiscalYear.FiscalPeriods.Count);
        Assert.Equal(new DateTime(2024, 3, 1), firstPeriod!.StartDate);
        Assert.Equal(new DateTime(2024, 3, 31), firstPeriod!.EndDate);
        Assert.Equal(new DateTime(2025, 2, 1), lastPeriod!.StartDate);
        Assert.Equal(new DateTime(2025, 2, lastDayOfFebruary), lastPeriod!.EndDate);
    }

    [Fact]
    public void Create_FiscalYear_Start_Dec()
    {
        // Arrange
        int lastDayOfFebruary = DateTime.IsLeapYear(2025) ? 29 : 28;

        FiscalYear fiscalYear = new
        (
            2024,
            new DateTime(2024, 12, 1),
            new DateTime(2025, 12, 31),
            true,
            false,
            false,
            DateTime.MaxValue,
            []
        );

        // Act
        fiscalYear.CreateFiscalPeriods();

        // Assert
        FiscalPeriod? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriod>();
        FiscalPeriod? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriod>();

        Assert.Equal(12, fiscalYear.FiscalPeriods.Count);
        Assert.Equal(new DateTime(2024, 12, 1), firstPeriod!.StartDate);
        Assert.Equal(new DateTime(2024, 12, 31), firstPeriod!.EndDate);
        Assert.Equal(new DateTime(2025, 11, 1), lastPeriod!.StartDate);
        Assert.Equal(new DateTime(2025, 11, 30), lastPeriod!.EndDate);
    }

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
