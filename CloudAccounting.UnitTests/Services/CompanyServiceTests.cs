using CloudAccounting.Application.Services;
using CloudAccounting.UnitTests.TestData;

namespace CloudAccounting.UnitTests.Services
{
    public class CompanyServiceTests
    {
        private readonly ICompanyRepository _repository = Substitute.For<ICompanyRepository>();
        private CompanyService _companyService => new CompanyService(_repository);

        [Fact]
        public void Create_FiscalYear_Start_Jan()
        {
            // Arrange
            FiscalYear fiscalYear = new
            (
                2,
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
            CompanyService.CreateFiscalPeriods(fiscalYear);

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
                2,
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
            CompanyService.CreateFiscalPeriods(fiscalYear);

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
                2,
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
            CompanyService.CreateFiscalPeriods(fiscalYear);

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
                2,
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
            CompanyService.CreateFiscalPeriods(fiscalYear);

            // Assert
            FiscalPeriod? firstPeriod = fiscalYear.FiscalPeriods.FirstOrDefault<FiscalPeriod>();
            FiscalPeriod? lastPeriod = fiscalYear.FiscalPeriods.LastOrDefault<FiscalPeriod>();

            Assert.Equal(12, fiscalYear.FiscalPeriods.Count);
            Assert.Equal(new DateTime(2024, 12, 1), firstPeriod!.StartDate);
            Assert.Equal(new DateTime(2024, 12, 31), firstPeriod!.EndDate);
            Assert.Equal(new DateTime(2025, 11, 1), lastPeriod!.StartDate);
            Assert.Equal(new DateTime(2025, 11, 30), lastPeriod!.EndDate);
        }

        [Fact]
        public async Task CompanyService_AddFiscalYear()
        {
            // Arrange
            Company company = CompanyTestData.GetCompanyForCreate();
            company.CompanyCode = 100;
            CompanyService companyService = new(_repository);

            int companyCode = 2;
            int fiscalYearNumber = 2025;
            int startMonthNumber = 2;

            // Act
            Result<FiscalYear> result = await companyService.CreateFiscalYearWithPeriods(companyCode, fiscalYearNumber, startMonthNumber);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value.FiscalPeriods);
            Assert.Equal(12, result.Value.FiscalPeriods.Count);
            Assert.Equal(new DateTime(2026, 1, 31), result.Value.FiscalPeriods[11].EndDate);
        }

    }
}