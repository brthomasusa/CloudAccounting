using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.IntegrationTests.LookupTests
{
    [Collection("SequentialTestCollection")]
    public class LookupRepositoryTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly AppDbContext _context = fixture.Context!;
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

        [Fact]
        public async Task RetrieveFiscalYearsAsync_LookupRepository_ShouldRetrieveAll_FiscalYearLookupItems()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            Result<List<FiscalYearLookupItem>> result = await _repo.RetrieveFiscalYearsAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count > 1);
        }

        [Fact]
        public async Task RetrieveFiscalPeriodsAsync_LookupRepository_ShouldRetrieveAll_FiscalPeriodLookupItems()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            Result<List<FiscalPeriodLookupItem>> result = await _repo.RetrieveFiscalPeriodsAsync(1, 2025);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count > 1);
        }

        [Fact]
        public async Task RetrieveVoucherTypesAsync_LookupRepository_ShouldRetrieveAll_VoucherTypeLookupItems()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            Result<List<VoucherTypeLookupItem>> result = await _repo.RetrieveVoucherTypesAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count > 1);
        }

        [Fact]
        public async Task RetrieveLedgerAccountsAsync_LookupRepository_ShouldRetrieveAll_CoaLookupItems()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            Result<List<CoaLookupItem>> result = await _repo.RetrieveLedgerAccountsAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count > 1);
        }
    }
}