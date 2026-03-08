#pragma warning disable CS9113

using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.IntegrationTests.LookupTests
{
    [Collection("SequentialTestCollection")]
    public class LookupEndpointTests(DatabaseFixture fixture, WebApplicationFactory<Program> factory)
        : IAsyncLifetime, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly CloudAccountingContext _context = fixture.Context!;
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        public class TestClass(DatabaseFixture fixture, WebApplicationFactory<Program> webApplicationFactory)
            : LookupEndpointTests(fixture, webApplicationFactory)
        {
            protected readonly JsonSerializerOptions? _options = new() { PropertyNameCaseInsensitive = true };
            private readonly HttpClient _client = webApplicationFactory.CreateClient();
            private const string relativePath = "/api/v1/lookups";

            [Fact]
            public async Task Get_CompanyLookupItems_ReturnsManyCompanyLookupItems()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);

                // Act
                using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/companycodes");
                response.EnsureSuccessStatusCode();
                List<CompanyLookupItem>? lookupItems = await response.Content.ReadFromJsonAsync<List<CompanyLookupItem>>(_options);

                // Assert
                int count = lookupItems!.Count;
                Assert.True(count > 1);
            }
        }
    }
}