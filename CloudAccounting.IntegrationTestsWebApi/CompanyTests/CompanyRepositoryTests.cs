using CompanyDataModel = CloudAccounting.Infrastructure.Data.Models.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class CompanyRepositoryTests : TestBase
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

        public CompanyRepositoryTests()
            => _companyRepo = new CompanyRepository(_efCoreContext!, _memoryCache!, new NullLogger<CompanyRepository>(), _dapperContext!, _mapper);

        [Fact]
        public async Task RetrieveAllAsync_CompanyRepository_ReturnsMultibleCompanies()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 5;

            // Act
            Result<List<Company>> result = await _companyRepo.RetrieveAllAsync(pageNumber, pageSize);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count > 1);
        }

        [Fact]
        public async Task RetrieveAsync_CompanyRepository_ReturnsOneCompany()
        {
            // Arrange
            int companyCode = 1;

            // Act
            Result<Company> result = await _companyRepo.RetrieveAsync(companyCode, new CancellationToken(), true);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("BTechnical Consulting", result.Value.CompanyName);
        }

        // [Fact]
        // public async Task RetrieveAsync_CompanyRepository_TestIfRetrievedCompanyWasCached()
        // {
        //     // Arrange
        //     CancellationToken token = new();
        //     int companyCode = 1;
        //     string cacheKey = $"company-{companyCode}";
        //     _memoryCache!.TryGetValue(cacheKey, out Company? beforeCached);

        //     // Act
        //     Result<Company> result = await _companyRepo.RetrieveAsync(companyCode, token, false);
        //     _memoryCache!.TryGetValue(cacheKey, out Company? afterCached);

        //     // Assert
        //     Assert.Null(beforeCached);
        //     Assert.NotNull(afterCached);
        //     Assert.Equal(afterCached.CompanyName, result.Value.CompanyName);
        // }

        // [Fact]
        // public async Task RetrieveAsync_CompanyRepository_ReturnsNotFound()
        // {
        //     // Arrange
        //     CancellationToken token = new();
        //     int companyCode = 3;

        //     // Act
        //     Result<Company> result = await _companyRepo.RetrieveAsync(companyCode, token, true);

        //     // Assert
        //     Assert.True(result.IsFailure);
        //     Assert.Equal("The company with CompanyCode '3' was not found.", result.Error.Message);
        // }

        [Fact]
        public async Task Create_CompanyRepository_CreatesAndReturnsOneCompany()
        {
            // Arrange
            Company companyToCreate = CompanyTestData.GetCompanyForCreate();

            // Act
            Result<Company> result = await _companyRepo.CreateAsync(companyToCreate);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyToCreate.CompanyName, result.Value.CompanyName);
        }

        [Fact]
        public async Task Update_CompanyRepository_UpdatesAndReturnsOneCompany()
        {
            // Arrange
            Company companyToUpdate = CompanyTestData.GetCompanyForUpdate();

            // Act
            Result<Company> result = await _companyRepo.UpdateAsync(companyToUpdate);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyToUpdate.CompanyName, result.Value.CompanyName);
        }

        [Fact]
        public async Task Delete_CompanyRepository_DeletesOneCompanyAndReturnsTrue()
        {
            // Arrange
            int companyCode = 2;

            // Act
            Result result = await _companyRepo.DeleteAsync(companyCode);
            Result<Company> deletedCompanyResult = await _companyRepo.RetrieveAsync(companyCode, new CancellationToken(), false);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(deletedCompanyResult.IsFailure);
        }
    }
}