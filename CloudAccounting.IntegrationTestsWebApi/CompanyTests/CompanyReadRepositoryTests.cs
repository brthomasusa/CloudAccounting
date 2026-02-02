using CloudAccounting.Infrastructure.Data.Repositories;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class CompanyReadRepositoryTests : TestBase
    {
        [Fact]
        public async Task GetFiscalYearDtoByIdAndYearAsync_CompanyReadRepository()
        {
            // Arrange
            int companyCode = 4;
            int fiscalYearNumber = 2024;
            CompanyReadRepository repo = new(_dapperContext!, new NullLogger<CompanyReadRepository>());

            // Act
            Result<CompanyWithFiscalPeriodsDto> result = await repo.GetFiscalYearDtoByIdAndYearAsync(companyCode, fiscalYearNumber);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}