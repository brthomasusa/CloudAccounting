using CloudAccounting.Application.UseCases.Coa.GetAll;
using CloudAccounting.Application.UseCases.Coa.GetFilterByAccount;
using CloudAccounting.Application.UseCases.Coa.GetByAccount;

namespace CloudAccounting.IntegrationTests.ChartOfAccountTest;

[Collection("SequentialTestCollection")]
public class CoaQueryHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    // private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;

    private IChartOfAccountRepository Repo =>
        new ChartOfAccountRepository(_context, new NullLogger<ChartOfAccountRepository>());

    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task RetrieveAllQueryHandler_ShouldReturnPagedResponse()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        const int companyCode = 1;
        const int pageNumber = 1;
        const int pageSize = 10;
        RetrieveAllQuery query = new(pageNumber, pageSize, companyCode);
        RetrieveAllQueryHandler handler = new(Repo, new NullLogger<RetrieveAllQueryHandler>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value.Data);
    }

    [Fact]
    public async Task RetrieveAllByAccountQueryHandler_ShouldReturnPagedResponse()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        const int companyCode = 1;
        const int pageNumber = 1;
        const int pageSize = 10;
        const string accountCode = "101";

        RetrieveAllByAccountQuery query = new(pageNumber, pageSize, companyCode, accountCode);
        RetrieveAllByAccountQueryHandler handler = new(Repo, new NullLogger<RetrieveAllByAccountQueryHandler>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value.Data);
    }

    [Fact]
    public async Task GetChartOfAccountByAccountCodeQueryHandler_ShouldReturnCoaDto()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        const int companyCode = 1;
        const string accountCode = "10100100001";

        GetChartOfAccountByAccountCodeQuery query = new(companyCode, accountCode);
        GetChartOfAccountByAccountCodeQueryHandler handler =
            new(Repo, new NullLogger<GetChartOfAccountByAccountCodeQueryHandler>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(accountCode, result.Value.AccountCode);
    }
}