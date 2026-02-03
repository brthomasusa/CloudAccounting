using CloudAccounting.Application.Validators.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.UseCases.UpdateCompany;
using CloudAccounting.Application.UseCases.DeleteCompany;
using CloudAccounting.Application.UseCases.Company.CreateFiscalYear;
using FluentValidation.TestHelper;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyValidatorTests : TestBase
{
    private readonly ICompanyReadRepository _repository;
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    public CompanyValidatorTests()
    {
        _repository = new CompanyReadRepository(_dapperContext!, new NullLogger<CompanyReadRepository>());
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

    [Fact]
    public async Task UpdateCompanyValidator_ShouldHaveOneValidationErrors_CompanyCodeIsZero()
    {
        // Arrange
        UpdateCompanyValidator validator = new(_repository);
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();
        command.CompanyCode = 0;

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyCode);
    }

    [Fact]
    public async Task UpdateCompanyValidator_ShouldHaveOneValidationErrors_CompanyCodeNotFound()
    {
        // Arrange
        UpdateCompanyValidator validator = new(_repository);
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();
        command.CompanyCode = 13;

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyCode);
    }

    [Fact]
    public async Task DeleteCompanyValidator_ShouldHaveOneValidationErrors_CompanyCodeNotFound()
    {
        // Arrange
        DeleteCompanyValidator validator = new(_repository);
        DeleteCompanyCommand command = new() { CompanyCode = 1 };

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task DeleteCompanyValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        DeleteCompanyValidator validator = new(_repository);
        DeleteCompanyCommand command = new() { CompanyCode = 13 };

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyCode);
    }

    [Fact]
    public async Task CreateFiscalYearCommandValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        CreateFiscalYearCommandValidator validator = new(_repository);
        CreateFiscalYearCommand command = new(1, 2025, 1);

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}
