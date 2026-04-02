namespace CloudAccounting.IntegrationTests.FiscalYearTests;

[Collection("SequentialTestCollection")]
public class FiscalYearRepositoryTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private IFiscalYearRepository _repo => new FiscalYearRepository(_context, new NullLogger<FiscalYearRepository>());
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task InsertFiscalYearAsync_FiscalYearRepository_ShouldInsert12FiscalYearRecords()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        FiscalYear fiscalYear = GetFiscalYearDomainModel();

        // Act
        Result<FiscalYear> result = await _repo.InsertFiscalYearAsync(fiscalYear);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(12, result.Value.FiscalPeriods.Count);
    }

    [Fact]
    public async Task GetMostRecentFiscalYearAsync_FiscalYearRepository_CompanyWithPrevFiscalYear_ShouldRetrieve12FiscalYearRecords()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;

        // Act
        Result<FiscalYear> result = await _repo.GetMostRecentFiscalYearAsync(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2025, result.Value.Year);
        Assert.Equal(12, result.Value.FiscalPeriods.Count);
    }

    [Fact]
    public async Task GetMostRecentFiscalYearAsync_FiscalYearRepository_CompanyWithoutPrevFiscalYear_ShouldRetrieveNoFiscalYearRecords()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 2;

        // Act
        Result<FiscalYear> result = await _repo.GetMostRecentFiscalYearAsync(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.Year);
        Assert.Empty(result.Value.FiscalPeriods);
    }

    [Fact]
    public async Task GetFiscalYearByCompanyAndYearAsync_FiscalYearRepository_ShouldRetrieve12FiscalYearRecords()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;
        int fiscalYearNumber = 2024;

        // Act
        Result<FiscalYear> result = await _repo.GetFiscalYearAsync(companyCode, fiscalYearNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(12, result.Value.FiscalPeriods.Count);
    }

    [Fact]
    public async Task CanFiscalYearBeDeleted_FiscalYearRepository_ShouldReturnTrue()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;
        int fiscalYearNumber = 2024;        // Has no transactions.

        // Act
        Result<bool> result = await _repo.CanFiscalYearBeDeleted(companyCode, fiscalYearNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task EarliestNextFiscalYearStartDate_FiscalYearRepository_ShouldReturn_20260701()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;

        // Act
        Result<DateTime> result = await _repo.EarliestNextFiscalYearStartDate(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new DateTime(2026, 7, 1), result.Value);
    }

    [Fact]
    public async Task EarliestNextFiscalYearStartDate_FiscalYearRepository_ShouldReturn_DateTime_MinVal()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 2;

        // Act
        Result<DateTime> result = await _repo.EarliestNextFiscalYearStartDate(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(DateTime.MinValue, result.Value);
    }

    [Fact]
    public async Task GetCompanyName_FiscalYearRepository_ShouldReturn_Contoso_Ltd()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 2;

        // Act
        Result<string> result = await _repo.GetCompanyName(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Contoso Ltd.", result.Value);
    }

    [Fact]
    public async Task DoesCompanyHaveInitialFiscalYear_FiscalYearRepository_ShouldReturn_True()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;

        // Act
        Result<bool> result = await _repo.DoesCompanyHaveInitialFiscalYear(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task DoesCompanyHaveInitialFiscalYear_FiscalYearRepository_ShouldReturn_False()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 2;

        // Act
        Result<bool> result = await _repo.DoesCompanyHaveInitialFiscalYear(companyCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }

    [Fact]
    public async Task IsUniqueFiscalYearNumber_FiscalYearRepository_ShouldReturn_False()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 1;
        int fiscalYearNumber = 2024;

        // Act
        Result<bool> result = await _repo.IsUniqueFiscalYearNumber(companyCode, fiscalYearNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }

    [Fact]
    public async Task IsUniqueFiscalYearNumber_FiscalYearRepository_ShouldReturn_True()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        int companyCode = 2;
        int fiscalYearNumber = 2025;

        // Act
        Result<bool> result = await _repo.IsUniqueFiscalYearNumber(companyCode, fiscalYearNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }


    private static FiscalYear GetFiscalYearDomainModel()
        => new(
                    2,
                    "Contoso Ltd.",
                    2025,
                    new DateTime(2025, 1, 1),
                    new DateTime(2025, 12, 31),
                    true,
                    false,
                    false,
                    DateTime.MinValue,
                [
                        new FiscalPeriod(1, "January", new DateTime(2025,1,1), new DateTime(2025,1,31), false),
                        new FiscalPeriod(2, "February", new DateTime(2025,2,1), new DateTime(2025,2,28), false),
                        new FiscalPeriod(3, "March", new DateTime(2025,3,1), new DateTime(2025,3,31), false),
                        new FiscalPeriod(4, "April", new DateTime(2025,4,1), new DateTime(2025,4,30), false),
                        new FiscalPeriod(5, "May", new DateTime(2025,5,1), new DateTime(2025,5,31), false),
                        new FiscalPeriod(6, "June", new DateTime(2025,6,1), new DateTime(2025,6,30), false),
                        new FiscalPeriod(7, "July", new DateTime(2025,7,1), new DateTime(2025,7,31), false),
                        new FiscalPeriod(8, "August", new DateTime(2025,8,1), new DateTime(2025,8,31), false),
                        new FiscalPeriod(9, "September", new DateTime(2025,9,1), new DateTime(2025,9,30), false),
                        new FiscalPeriod(10, "October", new DateTime(2025,10,1), new DateTime(2025,10,31), false),
                        new FiscalPeriod(11, "November", new DateTime(2025,11,1), new DateTime(2025,11,30), false),
                        new FiscalPeriod(12, "December", new DateTime(2025,12,1), new DateTime(2025,12,31), false)
                    ]
                );







}
