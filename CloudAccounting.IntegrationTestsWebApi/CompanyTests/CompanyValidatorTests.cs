using CloudAccounting.Application.Validators.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
using FluentValidation.TestHelper;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyValidatorTests : TestBase
{
    private readonly ICompanyRepository _repository;

    public CompanyValidatorTests()
    {
        _repository = new CompanyRepository(_dbContext!, _memoryCache!, new NullLogger<CompanyRepository>());
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
    public async Task CreateCompanyValidator_HasValidationError_CompanyNameIsNotUnique()
    {
        // Arrange
        CreateCompanyValidator validator = new(_repository);
        CreateCompanyCommand command = TestData.CompanyTestData.GetCreateCompanyCommand();
        command.CompanyName = "BTechnical Consulting";

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CompanyName);
    }
}
