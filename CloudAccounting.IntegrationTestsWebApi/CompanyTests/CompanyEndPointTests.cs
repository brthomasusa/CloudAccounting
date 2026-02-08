using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.UseCases.DeleteCompany;
using CloudAccounting.Application.UseCases.UpdateCompany;
using CloudAccounting.Application.UseCases.Company.CreateFiscalYear;
using CloudAccounting.Application.UseCases.Company.DeleteFiscalYear;
using CloudAccounting.Shared.Company;
using System.Net.Http.Headers;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class CompanyEndPointTests(WebApplicationFactory<Program> factory) : TestBase, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient = factory.CreateClient();

        private const string relativePath = "/api/v1/companies";

        public class TestClass(WebApplicationFactory<Program> webApplicationFactory) : CompanyEndPointTests(webApplicationFactory)
        {
            [Fact]
            public async Task Get_Companies_ReturnsManyCompanies()
            {
                // Act
                var queryParams = new Dictionary<string, string?>
                {
                    ["pageNumber"] = "1",
                    ["pageSize"] = "5"
                };

                List<CompanyDetailVm>? response = await _httpClient
                    .GetFromJsonAsync<List<CompanyDetailVm>>(QueryHelpers.AddQueryString($"{relativePath}", queryParams));

                // Assert
                int count = response!.Count;
                Assert.True(count > 1);
            }

            [Fact]
            public async Task Get_CompaniesById_ReturnsOneCompany()
            {
                // Arrange
                const int companyId = 1;

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/{companyId}");
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
                const int companyId = -3;

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/{companyId}");

                // Assert
                Assert.False(response.IsSuccessStatusCode);
            }

            [Fact]
            public async Task Create_Company()
            {
                // Arrange
                string uri = $"{relativePath}";
                CreateCompanyCommand command = CompanyTestData.GetCreateCompanyCommand();
                var jsonCompany = JsonSerializer.Serialize(command);
                var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

                // Act
                using HttpResponseMessage response = await _httpClient.PostAsync(uri, requestContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                CompanyDetailVm? company = JsonSerializer.Deserialize<CompanyDetailVm>(content, _options);

                // Assert
                Assert.Equal(command.CompanyName, company!.CompanyName);
            }

            [Fact]
            public async Task Update_Company()
            {
                // Arrange        
                UpdateCompanyCommand command = CompanyTestData.GetUpdateCompanyCommand();
                string uri = $"{relativePath}"; ;
                var jsonCompany = JsonSerializer.Serialize(command);
                var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

                // Act
                using HttpResponseMessage response = await _httpClient.PutAsync(uri, requestContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                CompanyDetailVm? company = JsonSerializer.Deserialize<CompanyDetailVm>(content, _options);

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                Assert.NotNull(company);
            }

            [Fact]
            public async Task Delete_Company()
            {
                // Arrange  
                DeleteCompanyCommand command = new() { CompanyCode = 2 };
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
                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                // Assert
                Assert.True(response.IsSuccessStatusCode);
            }

            [Fact]
            public async Task Get_GetFiscalYearForCompanyAndYear_ReturnsOneFiscalYearDtoWith12FiscalPeriods()
            {
                // Arrange
                const int companyCode = 4;
                const int fiscalYear = 2024;

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/{companyCode}/{fiscalYear}");
                response.EnsureSuccessStatusCode();
                CompanyWithFiscalPeriodsDto? dto = await response.Content.ReadFromJsonAsync<CompanyWithFiscalPeriodsDto>();

                // Assert
                Assert.NotNull(dto);
                Assert.Equal("Maulibu Bar & Grill", dto.CompanyName);
                Assert.Equal(12, dto.FiscalPeriods.Count);
            }

            [Fact]
            public async Task Get_GetCompanyWithoutFiscalYear_ReturnsOneCompanyDtoWithoutFiscalPeriods()
            {
                // Arrange
                const int companyCode = 3;

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/fiscalyear/{companyCode}");
                response.EnsureSuccessStatusCode();
                CompanyWithFiscalPeriodsDto? dto = await response.Content.ReadFromJsonAsync<CompanyWithFiscalPeriodsDto>();

                // Assert
                Assert.NotNull(dto);
                Assert.Equal("Rooms-2-Go", dto.CompanyName);
                Assert.Empty(dto.FiscalPeriods);
            }

            [Fact]
            public async Task Create_CompanyWithFiscalPeriodsDto_ReturnsOneFiscalYearDtoWith12FiscalPeriods()
            {
                // Arrange
                string uri = $"{relativePath}/fiscalyear";
                CreateFiscalYearCommand command = new(2, 2025, 11);
                var jsonCompany = JsonSerializer.Serialize(command);
                var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

                // Act
                using HttpResponseMessage response = await _httpClient.PostAsync(uri, requestContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                CompanyWithFiscalPeriodsDto? dto = await response.Content.ReadFromJsonAsync<CompanyWithFiscalPeriodsDto>();

                // Assert
                Assert.Equal("Computer Depot", dto!.CompanyName);
                Assert.Equal(12, dto.FiscalPeriods.Count);
            }

            [Fact]
            public async Task Get_GetCompanyLookus()
            {
                // Arrange

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/lookups");
                response.EnsureSuccessStatusCode();
                List<CompanyLookup>? lookups = await response.Content.ReadFromJsonAsync<List<CompanyLookup>>();

                // Assert
                Assert.NotNull(lookups);
                Assert.NotEmpty(lookups);
            }
            // const int companyCode = 3;
            [Fact]
            public async Task Get_GetNextValidFiscalYearStartDate_July_1_2026()
            {
                // Arrange
                const int companyCode = 4;

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/validstartdate/{companyCode}");
                response.EnsureSuccessStatusCode();
                DateTime startDate = await response.Content.ReadFromJsonAsync<DateTime>();

                // Assert
                Assert.Equal(new DateTime(2026, 7, 1), startDate);
            }

            [Fact]
            public async Task Get_GetNextValidFiscalYearStartDate_ReturnsDateTime_MinValue()
            {
                // Arrange
                const int companyCode = 2;

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/validstartdate/{companyCode}");
                response.EnsureSuccessStatusCode();
                DateTime startDate = await response.Content.ReadFromJsonAsync<DateTime>();

                // Assert
                Assert.Equal(DateTime.MinValue, startDate);
            }

            [Fact]
            public async Task DeleteFiscalYear_Company()
            {
                // Arrange  
                DeleteFiscalYearCommand command = new(4, 2025);
                string uri = $"{relativePath}/fiscalyear";

                var memStream = new MemoryStream();
                await JsonSerializer.SerializeAsync(memStream, command);
                memStream.Seek(0, SeekOrigin.Begin);

                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var requestContent = new StreamContent(memStream);
                request.Content = requestContent;
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Act
                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                // Assert
                Assert.True(response.IsSuccessStatusCode);
            }


        }
    }
}