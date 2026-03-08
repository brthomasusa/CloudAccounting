using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.IntegrationTests.LookupTests
{
    [Collection("SequentialTestCollection")]
    public class LookupRepositoryTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly CloudAccountingContext _context = fixture.Context!;
        private ILookupRepository _repo => new LookupRepository(_context, new NullLogger<LookupRepository>());
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task RetrieveAllAsync_LookupRepository_ShouldRetrieveAll_CompanyLookupItems()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            Result<List<CompanyLookupItem>> result = await _repo.RetrieveAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count > 1);
        }

    }
}