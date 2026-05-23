namespace CloudAccounting.IntegrationTests.ChartOfAccountTest;

[Collection("SequentialTestCollection")]
public class CoaRepositoryTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;

    private IChartOfAccountRepository _repo =>
        new ChartOfAccountRepository(_context, new NullLogger<ChartOfAccountRepository>());

    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task RetrieveAllAsync_ShouldReturnMultiple()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);

        // Act
        var result = await _repo.RetrieveAllAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task CreateRetrieveUpdateDelete_AccountLifecycle_Works()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);

        var coa = new ChartOfAccounts
        {
            CompanyCode = 1,
            AccountCode = "ZZ999",
            AccountTitle = "Integration Test Account",
            AccountLevel = 5,
            AccountClassification = "A",
            AccountType = "Asset",
            CostCenterCode = null
        };

        // Act - Create
        var createResult = await _repo.CreateAsync(coa);

        // Assert - Create
        Assert.True(createResult.IsSuccess);
        Assert.Equal(coa.AccountCode, createResult.Value.AccountCode);

        // Act - Retrieve
        var retrieveResult = await _repo.RetrieveAsync(1, "ZZ999");

        // Assert - Retrieve
        Assert.True(retrieveResult.IsSuccess);
        Assert.Equal("Integration Test Account", retrieveResult.Value.AccountTitle);

        // Act - Update
        retrieveResult.Value.AccountTitle = "Updated Title";
        var updateResult = await _repo.UpdateAsync(retrieveResult.Value);

        // Assert - Update
        Assert.True(updateResult.IsSuccess);
        Assert.Equal("Updated Title", updateResult.Value.AccountTitle);

        // Act - Delete
        var deleteResult = await _repo.DeleteAsync(1, "ZZ999");

        // Assert - Delete
        Assert.True(deleteResult.IsSuccess);
        var check = await _context.ChartOfAccounts.FindAsync(1, "ZZ999");
        Assert.Null(check);
    }

    [Fact]
    public async Task IsExistingAccount_ReturnsExpected()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);

        var coa = new ChartOfAccounts
        {
            CompanyCode = 1,
            AccountCode = "EXIST1",
            AccountTitle = "Exist Test",
            AccountLevel = 5
        };

        await _repo.CreateAsync(coa);

        // Act & Assert
        var exists = await _repo.IsExistingAccount(1, "EXIST1");
        Assert.True(exists.IsSuccess);
        Assert.True(exists.Value);

        var notExists = await _repo.IsExistingAccount(1, "NOPE");
        Assert.True(notExists.IsSuccess);
        Assert.False(notExists.Value);
    }

    [Fact]
    public async Task IsParentWithChildren_LongParent_ReturnsTrue_And_Short_ReturnsFalse()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);

        var parent = new ChartOfAccounts { CompanyCode = 1, AccountCode = "90000", AccountTitle = "Parent" };
        var child = new ChartOfAccounts { CompanyCode = 1, AccountCode = "900001", AccountTitle = "Child" };

        await _repo.CreateAsync(parent);
        await _repo.CreateAsync(child);

        // Act
        var parentCheck = await _repo.IsParentWithChildren(1, "90000");
        var shortCheck = await _repo.IsParentWithChildren(1, "900");

        // Assert
        Assert.True(parentCheck.IsSuccess);
        Assert.True(parentCheck.Value);

        Assert.True(shortCheck.IsSuccess);
        Assert.False(shortCheck.Value);
    }
}