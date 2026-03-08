using CloudAccounting.Shared.FiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.GetFiscalYear;

namespace CloudAccounting.IntegrationTests.FiscalYearTests
{
    [Collection("SequentialTestCollection")]
    public class FiscalYearQueryHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly CloudAccountingContext _context = fixture.Context!;
        private IFiscalYearRepository _repo => new FiscalYearRepository(_context, new NullLogger<FiscalYearRepository>());
        private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task Handle_GetFiscalYearByCompanyAndYearQueryHandler_ShouldReturn_1_CompanyWithFiscalPeriodDtos()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            GetFiscalYearQuery query = new(1, 2024);
            GetFiscalYearQueryHandler handler = new(_repo, new NullLogger<GetFiscalYearQueryHandler>());

            // Act
            Result<FiscalYearDto> result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("BTechnical Consulting", result.Value.CompanyName);
            Assert.True(result.Value.IsInitialFiscalYear);
            Assert.Equal(12, result.Value.FiscalPeriods.Count);
        }

        [Fact]
        public async Task Handle_GetCompanyWithNoFiscalYearDataQueryHandler_ShouldReturn_1_CompanyWithoutFiscalPeriodDtos()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            GetFiscalYearQuery query = new(2, 2024);
            GetFiscalYearQueryHandler handler = new(_repo, new NullLogger<GetFiscalYearQueryHandler>());

            // Act
            Result<FiscalYearDto> result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Contoso Ltd.", result.Value.CompanyName);
            ; Assert.Empty(result.Value.FiscalPeriods);
        }
    }
}