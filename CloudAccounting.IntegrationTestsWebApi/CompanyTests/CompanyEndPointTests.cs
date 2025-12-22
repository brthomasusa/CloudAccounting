using Microsoft.AspNetCore.Mvc.Testing;    // To use WebApplicationFactory<T>.
using System.Net.Http.Json;                 // To use ReadFromJsonAsync.
using CloudAccounting.EntityModels.Entities;
using CloudAccounting.IntegrationTestsWebApi.TestData;
using System.Text.Json;
using System.Text;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests;

public class CompanyEndPointTests(WebApplicationFactory<Program> factory) : TestBase, IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;
    private const string relativePath = "/api/v1/companies";

    [Fact]
    public async Task Get_Companies_ReturnsSuccessStatusCode()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync(relativePath);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Get_Companies_ReturnsTwoCompanies()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync(relativePath);
        Company[]? companies = await response.Content.ReadFromJsonAsync<Company[]>();

        // Assert
        Assert.NotNull(companies);
        Assert.Equal(2, companies.Length);
    }

    [Fact]
    public async Task Get_CompaniesById_ReturnsOneCompany()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        const int companyId = 1;

        // Act
        HttpResponseMessage response = await client.GetAsync($"{relativePath}/{companyId}");
        Company? company = await response.Content.ReadFromJsonAsync<Company>();

        // Assert
        Assert.NotNull(company);
        Assert.Equal("BTechnical Consulting", company.CompanyName);
    }

    [Fact]
    public async Task Create_Company()
    {
        // Arrange
        string uri = $"{relativePath}";
        Company company = CompanyTestData.GetCompanyForCreate();
        var jsonCompany = JsonSerializer.Serialize(company);
        var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

        // Act
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.PostAsync(uri, requestContent);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var newCompany = JsonSerializer.Deserialize<Company>(content, _options);

        // Assert
        Assert.Equal(company.CompanyName, newCompany!.CompanyName);
        Assert.Equal(company.Address, newCompany!.Address);
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
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.PutAsync(uri, requestContent);
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
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.DeleteAsync(uri);
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
}
