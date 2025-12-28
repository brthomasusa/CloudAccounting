using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class CompanyRepositoryTests : TestBase
    {
        [Fact]
        public async Task RetrieveAllAsync_CompanyRepository_ReturnsTwoCompanies()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 5;
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());

            // Act
            Result<List<Company>> result = await repo.RetrieveAllAsync(pageNumber, pageSize);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
        }

        [Fact]
        public async Task RetrieveAsync_CompanyRepository_ReturnsOneCompany()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
            CancellationToken token = new();
            int companyCode = 1;

            // Act
            Result<Company> result = await repo.RetrieveAsync(companyCode, token, true);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("BTechnical Consulting", result.Value.CompanyName);
        }

        [Fact]
        public async Task RetrieveAsync_CompanyRepository_TestIfRetrievedCompanyWasCached()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
            CancellationToken token = new();
            int companyCode = 1;
            string cacheKey = $"company-{companyCode}";
            _memoryCache!.TryGetValue(cacheKey, out Company? beforeCached);

            // Act
            Result<Company> result = await repo.RetrieveAsync(companyCode, token, false);
            _memoryCache!.TryGetValue(cacheKey, out Company? afterCached);

            // Assert
            Assert.Null(beforeCached);
            Assert.NotNull(afterCached);
            Assert.Equal(afterCached.CompanyName, result.Value.CompanyName);
        }

        [Fact]
        public async Task RetrieveAsync_CompanyRepository_ReturnsNotFound()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
            CancellationToken token = new();
            int companyCode = 3;

            // Act
            Result<Company> result = await repo.RetrieveAsync(companyCode, token, true);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("The company with CompanyCode '3' was not found.", result.Error.Message);
        }

        [Fact]
        public async Task Create_CompanyRepository_CreatesAndReturnsOneCompany()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
            Company companyToCreate = CompanyTestData.GetCompanyForCreate();

            // Act
            Result<Company> result = await repo.CreateAsync(companyToCreate);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyToCreate.CompanyName, result.Value.CompanyName);
        }

        [Fact]
        public async Task Update_CompanyRepository_UpdatesAndReturnsOneCompany()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
            Company companyToUpdate = CompanyTestData.GetCompanyForUpdate();

            // Act
            Result<Company> result = await repo.UpdateAsync(companyToUpdate);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyToUpdate.CompanyName, result.Value.CompanyName);
        }

        [Fact]
        public async Task Delete_CompanyRepository_DeletesOneCompanyAndReturnsTrue()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
            int companyCode = 2;

            // Act
            Result result = await repo.DeleteAsync(companyCode);
            Result<Company> deletedCompanyResult = await repo.RetrieveAsync(companyCode, new CancellationToken(), false);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(deletedCompanyResult.IsFailure);
        }

        [Fact]
        public async Task IsUniqueCompanyName_CompanyRepository_ShouldReturnsTrue()
        {
            // Arrange
            CompanyRepository repo = new(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
            string companyName = "Hello World";

            // Act
            Result<bool> result = await repo.IsUniqueCompanyName(companyName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }
    }
}