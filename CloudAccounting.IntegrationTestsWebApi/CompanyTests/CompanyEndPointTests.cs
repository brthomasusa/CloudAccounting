using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.CreateCompany;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class CompanyEndPointTests(WebApplicationFactory<Program> factory) : TestBase, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient = factory.CreateClient();

        private const string relativePath = "/api/v1/companies";

        public class TestClass(WebApplicationFactory<Program> webApplicationFactory) : CompanyEndPointTests(webApplicationFactory)
        {
            [Fact]
            public async Task Get_Companies_ReturnsTwoCompanies()
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
                Assert.Equal(2, count);
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
                const int companyId = 3;

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
                CompanyDetailVm? newCompany = JsonSerializer.Deserialize<CompanyDetailVm>(content, _options);

                // Assert
                Assert.Equal(command.CompanyName, newCompany!.CompanyName);
            }

            [Fact]
            public async Task Update_Company()
            {
                // Arrange        
                Company company = CompanyTestData.GetCompanyForUpdate();
                string uri = $"{relativePath}/{company.CompanyCode}";
                var jsonCompany = JsonSerializer.Serialize(company);
                var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

                // Act
                using HttpResponseMessage response = await _httpClient.PutAsync(uri, requestContent);
                response.EnsureSuccessStatusCode();

                // Assert
                Assert.True(response.IsSuccessStatusCode);
            }

            [Fact]
            public async Task Delete_Company()
            {
                // Arrange  
                int companyCode = 2;
                string uri = $"{relativePath}/{companyCode}";

                // Act
                using HttpResponseMessage response = await _httpClient.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();

                // Assert
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}