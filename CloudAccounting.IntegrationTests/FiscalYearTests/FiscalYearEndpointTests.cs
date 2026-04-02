#pragma warning disable CS9113

using CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear;
using CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;
using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.IntegrationTests.FiscalYearTests
{
    [Collection("SequentialTestCollection")]
    public class FiscalYearEndpointTests(DatabaseFixture fixture, WebApplicationFactory<Program> factory)
        : IAsyncLifetime, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly AppDbContext _context = fixture.Context!;
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        public class TestClass(DatabaseFixture fixture, WebApplicationFactory<Program> webApplicationFactory)
        : FiscalYearEndpointTests(fixture, webApplicationFactory)
        {
            protected readonly JsonSerializerOptions? _options = new() { PropertyNameCaseInsensitive = true };
            private readonly HttpClient _client = webApplicationFactory.CreateClient();
            private const string relativePath = "/api/v1/fiscalyears";

            [Fact]
            public async Task Get_FiscalYears_ReturnsManyFiscalYears()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                int companyCode = 1;
                int fiscalYear = 2024;

                // Act
                using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/{companyCode}/{fiscalYear}");
                response.EnsureSuccessStatusCode();
                FiscalYearDto? fiscalYears = await response.Content.ReadFromJsonAsync<FiscalYearDto>(_options);

                // Assert
                int count = fiscalYears!.FiscalPeriods.Count;
                Assert.Equal(12, count);
            }

            [Fact]
            public async Task Get_FiscalYearsById_ReturnsOneFiscalYear()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                const int fiscalYearId = 1;

                // Act
                using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/{fiscalYearId}");
                response.EnsureSuccessStatusCode();
                FiscalYear? fiscalYear = await response.Content.ReadFromJsonAsync<FiscalYear>();

                // Assert
                Assert.NotNull(fiscalYear);
                Assert.Equal(2025, fiscalYear.Year);
            }

            [Fact]
            public async Task Get_MostRecentFiscalYear_ReturnsOneFiscalYear_WithFiscalPeriods()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                const int companyCode = 1;

                // Act
                using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/{companyCode}");
                response.EnsureSuccessStatusCode();
                FiscalYear? fiscalYear = await response.Content.ReadFromJsonAsync<FiscalYear>();

                // Assert
                Assert.NotNull(fiscalYear);
                Assert.Equal(2025, fiscalYear.Year);
            }

            [Fact]
            public async Task Get_MostRecentFiscalYear_ReturnsOneFiscalYear_WithoutFiscalPeriods()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                const int companyCode = 2;

                // Act
                using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/{companyCode}");
                response.EnsureSuccessStatusCode();
                FiscalYear? fiscalYear = await response.Content.ReadFromJsonAsync<FiscalYear>();

                // Assert
                Assert.NotNull(fiscalYear);
                Assert.Equal(0, fiscalYear.Year);
            }

            [Fact]
            public async Task Create_CompanyWithFiscalPeriodsDto_ReturnsOneFiscalYearDtoWith12FiscalPeriods()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                string uri = $"{relativePath}";
                CreateFiscalYearCommand command = new(2, 2025, new DateTime(2025, 1, 1));
                var jsonCompany = JsonSerializer.Serialize(command);
                var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

                // Act
                using HttpResponseMessage response = await _client.PostAsync(uri, requestContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                FiscalYearDto? dto = await response.Content.ReadFromJsonAsync<FiscalYearDto>(_options);

                // Assert
                Assert.Equal("Contoso Ltd.", dto!.CompanyName);
                Assert.Equal(12, dto.FiscalPeriods.Count);
            }

            [Fact]
            public async Task DeleteFiscalYear_Company()
            {
                // Arrange  
                await ReseedTestDb.ReseedTestDbAsync(_context);
                DeleteFiscalYearCommand command = new(1, 2025);

                var memStream = new MemoryStream();
                await JsonSerializer.SerializeAsync(memStream, command);
                memStream.Seek(0, SeekOrigin.Begin);

                var request = new HttpRequestMessage(HttpMethod.Delete, relativePath);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var requestContent = new StreamContent(memStream);
                request.Content = requestContent;
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Act
                using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                // Assert
                Assert.True(response.IsSuccessStatusCode);
            }

        }
    }
}