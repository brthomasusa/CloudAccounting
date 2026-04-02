using CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;

namespace CloudAccounting.IntegrationTests.FiscalYearTests;

[Collection("SequentialTestCollection")]
public class FiscalYearCommandValidatorTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private IFiscalYearRepository _repo => new FiscalYearRepository(_context, new NullLogger<FiscalYearRepository>());

    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task CreateFiscalYearCommandValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateFiscalYearCommandValidator validator = new(_repo);
        CreateFiscalYearCommand command = new(2, 2025, new DateTime(2025, 5, 1));

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateFiscalYearCommandValidator_WithInvalidFiscalYearNumber_ShouldHaveValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateFiscalYearCommandValidator validator = new(_repo);
        CreateFiscalYearCommand command = new(1, 2025, new DateTime(2025, 5, 1));  // Fiscal year 2025 already exists for company code 1 in the seeded test database

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FiscalYear)
              .WithErrorMessage("The fiscal year number already exists for this company.");
    }

    [Fact]
    public async Task CreateFiscalYearCommandValidator_OverlappingDates_ShouldHaveValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateFiscalYearCommandValidator validator = new(_repo);
        CreateFiscalYearCommand command = new(1, 2026, new DateTime(2026, 5, 1));  // Fiscal year 2025 does not end until 2026-06-30

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartDate)
              .WithErrorMessage("The start date overlaps with an existing fiscal year for this company.");
    }

    [Fact]
    public async Task DeleteFiscalYearCommandValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteFiscalYearCommandValidator validator = new(_repo);
        DeleteFiscalYearCommand command = new(1, 2025);

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}
