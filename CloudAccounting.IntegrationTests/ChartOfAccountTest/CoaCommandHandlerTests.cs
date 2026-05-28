using CloudAccounting.Application.UseCases.Coa.Create;
using CloudAccounting.Application.UseCases.Coa.Delete;
using CloudAccounting.Application.UseCases.Coa.Update;
using static CloudAccounting.IntegrationTests.AddMapsterForTests;

namespace CloudAccounting.IntegrationTests.ChartOfAccountTest;

[Collection("SequentialTestCollection")]
public class CoaCommandHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;

    private IChartOfAccountRepository Repo =>
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
            new CreateChartOfAccountCommandHandler(Repo, new NullLogger<CreateChartOfAccountCommandHandler>(),
                GetMapper());

        // Act - Create
        var createResult = await handler.Handle(command, CancellationToken.None);

        // Assert - Create
        Assert.True(createResult.IsSuccess);
        Assert.NotNull(createResult.Value);
        Assert.Equal("Test Account", createResult.Value.AccountTitle);

        // Act - Retrieve
        var retrieveResult = await Repo.RetrieveAsync(command.CompanyCode,
            $"{command.LevelOne}{command.LevelTwo}{command.LevelThree}{command.LevelFour}");

        // Assert - Retrieve
        Assert.NotNull(retrieveResult);
        Assert.Equal("Test Account", retrieveResult.Value.AccountTitle);
    }

    [Fact]
    public async Task UpdateChartOfAccountCommandHandler_ShouldUpdateExistingCoa()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        var updateCommand = new UpdateChartOfAccountCommand(1, "30100200002", "Updated Test Account", "Other", "09001");

        var handler =
            new UpdateChartOfAccountCommandHandler(Repo, new NullLogger<UpdateChartOfAccountCommandHandler>());

        // Act
        var updateResult = await handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.NotNull(updateResult.Value);
        Assert.Equal("Updated Test Account", updateResult.Value.AccountTitle);
    }

    [Fact]
    public async Task DeleteChartOfAccountCommandHandler_ShouldDeleteExistingCoa()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        var deleteCommand = new DeleteChartOfAccountCommand(1, "30100200002");

        var handler =
            new DeleteChartOfAccountCommandHandler(Repo, new NullLogger<DeleteChartOfAccountCommandHandler>());

        // Act
        var deleteResult = await handler.Handle(deleteCommand, CancellationToken.None);

        // Assert
        Assert.True(deleteResult.IsSuccess);

        // Verify deletion
        var retrieveResult = await Repo.RetrieveAsync(deleteCommand.CompanyCode, deleteCommand.AccountCode);
        Assert.True(retrieveResult.IsFailure);
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