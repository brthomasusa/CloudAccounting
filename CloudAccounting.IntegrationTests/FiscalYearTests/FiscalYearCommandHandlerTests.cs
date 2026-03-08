using CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;
using CloudAccounting.Infrastructure.Data.Services;
using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.IntegrationTests.FiscalYearTests;

[Collection("SequentialTestCollection")]
public class FiscalYearCommandHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly CloudAccountingContext _context = fixture.Context!;
    private IFiscalYearRepository _repo => new FiscalYearRepository(_context, new NullLogger<FiscalYearRepository>());
    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task Handle_CreateFiscalYearCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateFiscalYearCommand command = new(2, 2025, new DateTime(2025, 5, 1));
        FiscalYearService service = new(_repo);
        CreateFiscalYearCommandHandler handler = new(service, new NullLogger<CreateFiscalYearCommandHandler>(), _mapper);

        // Act
        Result<FiscalYearDto> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsInitialFiscalYear);
        Assert.False(result.Value.HasTransactions);
        Assert.Equal(12, result.Value.FiscalPeriods.Count);
    }

    [Fact]
    public async Task Handle_DeleteFiscalYearCommandHandler_GivenValidCmd_ShouldSucceed()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        DeleteFiscalYearCommand command = new(1, 2025);
        DeleteFiscalYearCommandHandler handler = new(_repo, new NullLogger<DeleteFiscalYearCommandHandler>());

        // Act
        Result<MediatR.Unit> result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.IsSuccess);
    }
}
