using CloudAccounting.EntityModels.Entities;
using CloudAccounting.IntegrationTestsWebApi.TestData;
using CloudAccounting.Repositories.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class CompanyRepositoryTests : TestBase
    {
        [Fact]
        public async Task RetrieveAllAsync_CompanyRepository_ReturnsTwoCompanies()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!);

            // Act
            Company[] companies = await repo.RetrieveAllAsync();

            // Assert
            Assert.Equal(2, companies.Length);
        }

        [Fact]
        public async Task RetrieveAsync_CompanyRepository_ReturnsOneCompany()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!);
            CancellationToken token = new();
            int companyCode = 1;

            // Act
            Company? company = await repo.RetrieveAsync(companyCode, token, true);

            // Assert
            Assert.NotNull(company);
            Assert.Equal("BTechnical Consulting", company.CompanyName);
        }

        [Fact]
        public async Task RetrieveAsync_CompanyRepository_TestIfRetrievedCompanyWasCached()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!);
            CancellationToken token = new();
            int companyCode = 1;
            string cacheKey = $"company-{companyCode}";
            _memoryCache!.TryGetValue(cacheKey, out Company? beforeCached);

            // Act
            Company? company = await repo.RetrieveAsync(companyCode, token, false);
            _memoryCache!.TryGetValue(cacheKey, out Company? afterCached);

            // Assert
            Assert.Null(beforeCached);
            Assert.NotNull(afterCached);
            Assert.Equal(afterCached.CompanyName, company!.CompanyName);
        }

        [Fact]
        public async Task Create_CompanyRepository_CreatesAndReturnsOneCompany()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!);
            Company companyToCreate = CompanyTestData.GetCompanyForCreate();

            // Act
            Company? afterCreation = await repo.CreateAsync(companyToCreate);

            // Assert
            Assert.NotNull(afterCreation);
            Assert.Equal(companyToCreate.CompanyName, afterCreation.CompanyName);
        }

        [Fact]
        public async Task Update_CompanyRepository_UpdatesAndReturnsOneCompany()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!);
            Company companyToUpdate = CompanyTestData.GetCompanyForUpdate();

            // Act
            Company? afterUpdate = await repo.UpdateAsync(companyToUpdate);

            // Assert
            Assert.NotNull(afterUpdate);
            Assert.Equal(companyToUpdate.CompanyName, afterUpdate.CompanyName);
        }

        [Fact]
        public async Task Delete_CompanyRepository_DeletesOneCompanyAndReturnsTrue()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!);
            int companyCode = 2;

            // Act
            bool success = await repo.DeleteAsync(companyCode);
            Company? company = await repo.RetrieveAsync(companyCode, new CancellationToken(), false);

            // Assert
            Assert.True(success);
            Assert.Null(company);
        }



    }
}