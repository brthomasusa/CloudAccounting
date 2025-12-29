using CloudAccounting.Application.Validators.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
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

}
