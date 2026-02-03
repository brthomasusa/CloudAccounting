using CloudAccounting.Shared.Company;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class CompanyReadRepositoryTests : TestBase
    {
        private ICompanyReadRepository _repository => new CompanyReadRepository(_dapperContext!, new NullLogger<CompanyReadRepository>());

        [Fact]
        public async Task GetFiscalYearDtoByIdAndYearAsync_CompanyReadRepository()
        {
            // Arrange
            int companyCode = 4;
            int fiscalYearNumber = 2024;

            // Act
            Result<CompanyWithFiscalPeriodsDto> result = await _repository.GetFiscalYearDtoByIdAndYearAsync(companyCode, fiscalYearNumber);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task IsUniqueCompanyNameForCreate_CompanyReadRepository_ShouldReturnsTrue()
        {
            // Arrange
            string companyName = "Hello World";

            // Act
            Result<bool> result = await _repository.IsUniqueCompanyNameForCreate(companyName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsUniqueCompanyNameForCreate_CompanyReadRepository_ShouldReturnsFalse()
        {
            // Arrange
            string companyName = "BTechnical Consulting";

            // Act
            Result<bool> result = await _repository.IsUniqueCompanyNameForCreate(companyName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task IsUniqueCompanyNameForUpdate_CompanyReadRepository_ShouldReturnsTrue_NameChange()
        {
            // Arrange
            int companyCode = 1;
            string companyName = "New World Importers";   // An update is being performed which changes the company name

            // Act
            Result<bool> result = await _repository.IsUniqueCompanyNameForUpdate(companyCode, companyName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsUniqueCompanyNameForUpdate_CompanyReadRepository_ShouldReturnsTrue_NoNameChange()
        {
            // Arrange
            int companyCode = 1;
            string companyName = "BTechnical Consulting";   // An update is being perform which does not involve changing the company name

            // Act
            Result<bool> result = await _repository.IsUniqueCompanyNameForUpdate(companyCode, companyName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsUniqueCompanyNameForUpdate_CompanyReadRepository_ShouldReturnsFalse_DupeName()
        {
            // Arrange
            int companyCode = 1;
            string companyName = "Computer Depot";  // Another company already has this name.

            // Act
            Result<bool> result = await _repository.IsUniqueCompanyNameForUpdate(companyCode, companyName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task IsExistingCompany_CompanyReadRepository_ShouldReturnsTrue()
        {
            // Arrange
            int companyCode = 1;

            // Act
            Result<bool> result = await _repository.IsExistingCompany(companyCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsExistingCompany_CompanyReadRepository_ShouldReturnsFalse()
        {
            // Arrange
            int companyCode = 13;

            // Act
            Result<bool> result = await _repository.IsExistingCompany(companyCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task InitialFiscalYearExist_CompanyReadRepository_ShouldReturnsFalse()
        {
            // Arrange
            int companyCode = 2;

            // Act
            Result<bool> result = await _repository.InitialFiscalYearExist(companyCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task InitialFiscalYearExist_CompanyReadRepository_ShouldReturnsTrue()
        {
            // Arrange
            int companyCode = 4;

            // Act
            Result<bool> result = await _repository.InitialFiscalYearExist(companyCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task GetCompanyName_CompanyReadRepository()
        {
            // Arrange
            int companyCode = 4;

            // Act
            Result<string> result = await _repository.GetCompanyName(companyCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Maulibu Bar & Grill", result.Value);
        }
    }
}