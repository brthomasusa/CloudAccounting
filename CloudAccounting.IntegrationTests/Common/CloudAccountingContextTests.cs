namespace CloudAccounting.IntegrationTests.Common;

[Collection("SequentialTestCollection")]
public class CloudAccountingContextTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task CanRetrieveCompanies()
    {
        // Arrange
        await ReseedTestDbAsync();

        // Act
        var companies = await _context.Companies.ToListAsync();

        // Assert
        Assert.NotNull(companies);
        Assert.NotEmpty(companies);
    }

    [Fact]
    public async Task CanRetrieveFiscalYears()
    {
        // Arrange
        await ReseedTestDbAsync();

        // Act
        var fiscalYears = await _context.FiscalYears.ToListAsync();

        // Assert
        Assert.NotNull(fiscalYears);
        Assert.NotEmpty(fiscalYears);
    }

    [Fact]
    public async Task CanRetrieveVouchers()
    {
        // Arrange
        await ReseedTestDbAsync();

        // Act
        var vouchers = await _context.Vouchers.ToListAsync();

        // Assert
        Assert.NotNull(vouchers);
        Assert.NotEmpty(vouchers);
    }

    private async Task ReseedTestDbAsync()
        => await _context.Database.ExecuteSqlRawAsync("dbo.usp_ReseedCloudAcctgTestDb;");
}
