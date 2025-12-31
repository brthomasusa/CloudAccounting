using CloudAccounting.Application.Validators.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.UseCases.UpdateCompany;
using FluentValidation.TestHelper;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyValidatorTests : TestBase
{
    private readonly ICompanyRepository _repository;

    public CompanyValidatorTests()
    {
        _repository = new CompanyRepository(_efCoreContext!, _memoryCache!, new NullLogger<CompanyRepository>(), _dapperContext!);
    }

    [Fact]
    public async Task CreateCompanyValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        CreateCompanyValidator validator = new(_repository);
        CreateCompanyCommand command = TestData.CompanyTestData.GetCreateCompanyCommand();

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task UpdateCompanyValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        UpdateCompanyValidator validator = new(_repository);
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task UpdateCompanyValidator_ShouldHaveNoValidationErrors_NewNameButNotDupe()
    {
        // Arrange
        UpdateCompanyValidator validator = new(_repository);
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();
        command.CompanyName = "The Mayflower Company";
        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task UpdateCompanyValidator_ShouldHaveOneValidationErrors_DupeCompanyName()
    {
        // Arrange
        UpdateCompanyValidator validator = new(_repository);
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();
        command.CompanyName = "Computer Depot";

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyName);
    }

}
