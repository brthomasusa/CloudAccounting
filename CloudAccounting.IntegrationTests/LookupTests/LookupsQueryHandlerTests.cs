using CloudAccounting.Shared.Lookups;
using CloudAccounting.Application.UseCases.Lookups.GetCompanyLookupItems;

namespace CloudAccounting.IntegrationTests.LookupTests
{
    [Collection("SequentialTestCollection")]
    public class LookupsQueryHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly CloudAccountingContext _context = fixture.Context!;
        private ILookupRepository _repo => new LookupRepository(_context, new NullLogger<LookupRepository>());
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task Handle_GetCompanyLookupItemsQuery_ShouldRetrieveAll_CompanyLookupItems()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            GetCompanyLookupItemsQuery query = new();
            GetCompanyLookupItemsQueryHandler handler = new(_repo, new NullLogger<GetCompanyLookupItemsQueryHandler>());

            Result<List<CompanyLookupItem>> result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count > 1);
        }
    }
}