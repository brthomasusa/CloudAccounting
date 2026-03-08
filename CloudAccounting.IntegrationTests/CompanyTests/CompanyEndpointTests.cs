#pragma warning disable CS9113

using CloudAccounting.Shared.Company;
using CloudAccounting.Application.UseCases.Companies.CreateCompany;
using CloudAccounting.Application.UseCases.Companies.UpdateCompany;
using CloudAccounting.Application.UseCases.Companies.DeleteCompany;

namespace CloudAccounting.IntegrationTests.CompanyTests;

[Collection("SequentialTestCollection")]
public class CompanyEndpointTests(DatabaseFixture fixture, WebApplicationFactory<Program> factory)
    : IAsyncLifetime, IClassFixture<WebApplicationFactory<Program>>
{
    private readonly CloudAccountingContext _context = fixture.Context!;
    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    public class TestClass(DatabaseFixture fixture, WebApplicationFactory<Program> webApplicationFactory)
        : CompanyEndpointTests(fixture, webApplicationFactory)
    {
        protected readonly JsonSerializerOptions? _options = new() { PropertyNameCaseInsensitive = true };
        private readonly HttpClient _client = webApplicationFactory.CreateClient();
        private const string relativePath = "/api/v1/companies";

        [Fact]
        public async Task Get_Companies_ReturnsManyCompanies()
        {
            // Act
            await ReseedTestDb.ReseedTestDbAsync(_context);

            List<CompanyDetailDto>? response = await _client
                .GetFromJsonAsync<List<CompanyDetailDto>>(relativePath);

            // Assert
            int count = response!.Count;
            Assert.True(count > 1);
        }

        [Fact]
        public async Task Get_CompaniesById_ReturnsOneCompany()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            const int companyId = 1;

            // Act
            using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/{companyId}");
            response.EnsureSuccessStatusCode();
            Company? company = await response.Content.ReadFromJsonAsync<Company>();

            // Assert
            Assert.NotNull(company);
            Assert.Equal("BTechnical Consulting", company.CompanyName);
        }

        [Fact]
        public async Task Get_CompaniesById_ReturnsNotFoundResult()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            const int companyId = -3;

            // Act
            using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/{companyId}");

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Create_Company()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            string uri = $"{relativePath}";
            CreateCompanyCommand command = CompanyTestData.GetCreateCompanyCommand();
            var jsonCompany = JsonSerializer.Serialize(command);
            var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

            // Act
            using HttpResponseMessage response = await _client.PostAsync(uri, requestContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            CompanyDetailDto? company = JsonSerializer.Deserialize<CompanyDetailDto>(content, _options);

            // Assert
            Assert.Equal(command.CompanyName, company!.CompanyName);
        }

        [Fact]
        public async Task Update_Company()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            UpdateCompanyCommand command = CompanyTestData.GetUpdateCompanyCommand();
            string uri = $"{relativePath}"; ;
            var jsonCompany = JsonSerializer.Serialize(command);
            var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

            // Act
            using HttpResponseMessage response = await _client.PutAsync(uri, requestContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            CompanyDetailDto? company = JsonSerializer.Deserialize<CompanyDetailDto>(content, _options);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(company);
        }

        [Fact]
        public async Task Delete_Company()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            DeleteCompanyCommand command = new() { CompanyCode = 3 };
            string uri = $"{relativePath}";

            var memStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memStream, command);
            memStream.Seek(0, SeekOrigin.Begin);

            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
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
