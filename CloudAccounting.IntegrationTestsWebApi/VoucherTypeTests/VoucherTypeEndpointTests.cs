using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.IntegrationTestsWebApi.VoucherTypeTests
{
    public class VoucherTypeEndpointTests(WebApplicationFactory<Program> factory) : TestBase, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient = factory.CreateClient();

        private const string relativePath = "/api/v1/vouchertypes";

        public class TestClass(WebApplicationFactory<Program> webApplicationFactory) : VoucherTypeEndpointTests(webApplicationFactory)
        {
            [Fact]
            public async Task GetAllVoucherTypes_ReturnsAllVoucherTypes()
            {
                // Arrange

                // Act
                List<VoucherTypeDto>? response = await _httpClient
                    .GetFromJsonAsync<List<VoucherTypeDto>>($"{relativePath}");

                // Assert
                int count = response!.Count;
                Assert.True(count > 1);
            }

            [Fact]
            public async Task GetVoucherTypeById_ReturnsOneVoucherTypeDto()
            {
                // Arrange
                const int voucherCode = 1;

                // Act
                using HttpResponseMessage response = await _httpClient.GetAsync($"{relativePath}/{voucherCode}");
                response.EnsureSuccessStatusCode();
                VoucherTypeDto? voucher = await response.Content.ReadFromJsonAsync<VoucherTypeDto>();

                // Assert
                Assert.NotNull(voucher);
                Assert.Equal("BPV", voucher!.VoucherType);
            }
        }
    }
}