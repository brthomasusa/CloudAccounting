using CloudAccounting.Application.UseCases.Companies.CreateCompany;
using CloudAccounting.Application.UseCases.Companies.UpdateCompany;
using CloudAccounting.Application.UseCases.Companies.DeleteCompany;

namespace CloudAccounting.IntegrationTests.CompanyTests;

[Collection("SequentialTestCollection")]
public class CompanyValidatorTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly CloudAccountingContext _context = fixture.Context!;
    private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
    private ICompanyRepository _repo => new CompanyRepository(_context, _memoryCache!, new NullLogger<CompanyRepository>(), _mapper);
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task CreateCompanyValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateCompanyValidator validator = new(_repo);
        CreateCompanyCommand command = TestData.CompanyTestData.GetCreateCompanyCommand();

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateCompanyValidator_ShouldHaveValidationErrors_DuplicateCompanyName()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateCompanyValidator validator = new(_repo);
        CreateCompanyCommand command = CompanyTestData.GetCreateCompanyCommand();
        command.CompanyName = "BTechnical Consulting"; // This company name already exists in the seeded test database  

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CompanyName)
              .WithErrorMessage("This company name already exists.");
    }

    [Fact]
    public async Task UpdateCompanyValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        UpdateCompanyValidator validator = new(_repo);
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
        await ReseedTestDb.ReseedTestDbAsync(_context);
        UpdateCompanyValidator validator = new(_repo);
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
        await ReseedTestDb.ReseedTestDbAsync(_context);
        UpdateCompanyValidator validator = new(_repo);
        UpdateCompanyCommand command = TestData.CompanyTestData.GetUpdateCompanyCommand();
        command.CompanyName = "Contoso Ltd.";

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyName);
    }

    [Fact]
    public async Task UpdateCompanyValidator_ShouldHaveOneValidationErrors_CompanyCodeIsZero()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        UpdateCompanyValidator validator = new(_repo);
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
        await ReseedTestDb.ReseedTestDbAsync(_context);
        UpdateCompanyValidator validator = new(_repo);
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
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteCompanyValidator validator = new(_repo);
        DeleteCompanyCommand command = new() { CompanyCode = 11 };

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyCode);
    }

    [Fact]
    public async Task DeleteCompanyValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteCompanyValidator validator = new(_repo);
        DeleteCompanyCommand command = new() { CompanyCode = 3 };

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task DeleteCompanyValidator_ShouldHaveOneValidationErrors_CompanyHasFiscalYears()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteCompanyValidator validator = new(_repo);
        DeleteCompanyCommand command = new() { CompanyCode = 1 };

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyCode);
    }
}
