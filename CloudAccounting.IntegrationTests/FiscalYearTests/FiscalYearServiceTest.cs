using CloudAccounting.Core.Services;
using CloudAccounting.Infrastructure.Data.Services;

namespace CloudAccounting.IntegrationTests.FiscalYearTests;

[Collection("SequentialTestCollection")]
public class FiscalYearServiceTest(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly CloudAccountingContext _context = fixture.Context!;
    private IFiscalYearRepository _repo => new FiscalYearRepository(_context, new NullLogger<FiscalYearRepository>());
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task CreateFiscalYear_FiscalYearService_ShouldCreateAndInsert12FiscalYearRecords()
    {
        // Arrange
        IFiscalYearService service = new FiscalYearService(_repo);
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 2;
        int fiscalYearNumber = 2025;
        DateTime startDate = new(2025, 8, 1);

        // Act
        Result<FiscalYear> result = await service.CreateFiscalYear(companyCode, fiscalYearNumber, startDate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(12, result.Value.FiscalPeriods.Count);
    }

}
