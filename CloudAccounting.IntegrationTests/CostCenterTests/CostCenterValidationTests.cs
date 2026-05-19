using CloudAccounting.Application.UseCases.CostCenters.CreateCostCenter;
using CloudAccounting.Application.UseCases.CostCenters.DeleteCostCenters;

namespace CloudAccounting.IntegrationTests.CostCenterTests;

[Collection("SequentialTestCollection")]
public class CostCenterValidationTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private ICostCenterRepository _repo => new CostCenterRepository(_context, new NullLogger<CostCenterRepository>());
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task CreateCostCenterValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateCostCenterCommandValidator validator = new(_repo);
        var command = GetCostCenterCommandForCreate();

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("0")]
    [InlineData("000")]
    [InlineData("0000")]
    [InlineData("000000")]
    public async Task CreateCostCenterValidator_ShouldHaveValidationErrors_WhenCostCenterCodeIsInvalid(
        string invalidCode)
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateCostCenterCommandValidator validator = new(_repo);
        var command = GetCostCenterCommandForCreate();
        var updated = command with { CostCenterCode = invalidCode };

        // Act
        var result = await validator.TestValidateAsync(updated);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CostCenterCode);
    }

    [Fact]
    public async Task DeleteCostCenterValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteCostCenterCommandValidator validator = new(_repo);
        var command = new DeleteCostCenterCommand(1, "01001");

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task DeleteCostCenterValidator_WhenCostCenterHasChildren_ShouldHaveValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteCostCenterCommandValidator validator = new(_repo);
        var command = new DeleteCostCenterCommand(1, "01");

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyCode)
            .WithErrorMessage("This cost center has child cost centers and cannot be deleted.");
    }


    private static CreateCostCenterCommand GetCostCenterCommandForCreate()
    {
        return new CreateCostCenterCommand(0, "99", "Test Cost Center");
    }
}