using CloudAccounting.Application.UseCases.Coa.Create;
using CloudAccounting.Application.Mappings;
using static CloudAccounting.IntegrationTests.AddMapsterForTests;

namespace CloudAccounting.IntegrationTests.ChartOfAccountTest;

[Collection("SequentialTestCollection")]
public class CoaCommandHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;

    private IChartOfAccountRepository _repo =>
        new ChartOfAccountRepository(_context, new NullLogger<ChartOfAccountRepository>());

    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task CreateChartOfAccountCommandHandler_ShouldCreateAndRetrieve()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        var command = CreateValidCommand();
        var handler =
            new CreateChartOfAccountCommandHandler(_repo, new NullLogger<CreateChartOfAccountCommandHandler>(),
                GetMapper());

        // Act - Create
        var createResult = await handler.Handle(command, CancellationToken.None);

        // Assert - Create
        Assert.True(createResult.IsSuccess);
        Assert.NotNull(createResult.Value);
        Assert.Equal("Test Account", createResult.Value.AccountTitle);

        // Act - Retrieve
        var retrieveResult = await _repo.RetrieveAsync(command.CompanyCode,
            $"{command.LevelOne}{command.LevelTwo}{command.LevelThree}{command.LevelFour}");

        // Assert - Retrieve
        Assert.NotNull(retrieveResult);
        Assert.Equal("Test Account", retrieveResult.Value.AccountTitle);
    }

    private ChartOfAccounts CreateValidCoa()
    {
        return new ChartOfAccounts
        {
            CompanyCode = 1,
            AccountCode = "30100200003",
            AccountTitle = "Test Account",
            AccountLevel = 4,
            AccountClassification = "Assets",
            AccountType = "Other",
            CostCenterCode = "09001"
        };
    }

    private CreateChartOfAccountCommand CreateValidCommand()
    {
        return new CreateChartOfAccountCommand
        {
            CompanyCode = 1,
            LevelOne = "3",
            LevelTwo = "01",
            LevelThree = "002",
            LevelFour = "00003",
            AccountTitle = "Test Account",
            AccountType = "Other",
            CostCenterCode = "09001"
        };
    }
}