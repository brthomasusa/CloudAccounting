#pragma warning disable CS9113

using CloudAccounting.Shared.VoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypeById;
using CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypes;
using CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType;


namespace CloudAccounting.IntegrationTests.VoucherTypeTests
{
    [Collection("SequentialTestCollection")]
    public class VoucherTypeEndpointTests(DatabaseFixture fixture, WebApplicationFactory<Program> factory)
    : IAsyncLifetime, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly CloudAccountingContext _context = fixture.Context!;
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        public class TestClass(DatabaseFixture fixture, WebApplicationFactory<Program> webApplicationFactory)
            : VoucherTypeEndpointTests(fixture, webApplicationFactory)
        {
            protected readonly JsonSerializerOptions? _options = new() { PropertyNameCaseInsensitive = true };
            private readonly HttpClient _client = webApplicationFactory.CreateClient();
            private const string relativePath = "/api/v1/vouchertypes";

            [Fact]
            public async Task Get_VoucherTypes_ReturnsManyVoucherTypes()
            {
                // Act
                await ReseedTestDb.ReseedTestDbAsync(_context);

                List<VoucherTypeDto>? response = await _client
                    .GetFromJsonAsync<List<VoucherTypeDto>>(relativePath);

                // Assert
                int count = response!.Count;
                Assert.True(count > 1);
            }

            [Fact]
            public async Task Get_VoucherTypesById_ReturnsOneVoucherType()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                const int voucherTypeId = 1;

                // Act
                using HttpResponseMessage response = await _client.GetAsync($"{relativePath}/{voucherTypeId}");
                response.EnsureSuccessStatusCode();
                VoucherTypeDto? voucherType = await response.Content.ReadFromJsonAsync<VoucherTypeDto>();

                // Assert
                Assert.NotNull(voucherType);
                Assert.Equal("BPV", voucherType.VoucherType);
            }

            [Fact]
            public async Task Create_VoucherType()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                string uri = $"{relativePath}";
                CreateVoucherTypeCommand command = GetVoucherForCreate();
                var jsonCompany = JsonSerializer.Serialize(command);
                var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

                // Act
                using HttpResponseMessage response = await _client.PostAsync(uri, requestContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                VoucherTypeDto? voucher = JsonSerializer.Deserialize<VoucherTypeDto>(content, _options);

                // Assert
                Assert.Equal(command.VoucherType, voucher!.VoucherType);
            }

            [Fact]
            public async Task Update_VoucherType()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                UpdateVoucherTypeCommand command = GetVoucherForUpdate();
                string uri = $"{relativePath}"; ;
                var jsonCompany = JsonSerializer.Serialize(command);
                var requestContent = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

                // Act
                using HttpResponseMessage response = await _client.PutAsync(uri, requestContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                VoucherTypeDto? voucher = JsonSerializer.Deserialize<VoucherTypeDto>(content, _options);

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                Assert.NotNull(voucher);
            }

            [Fact]
            public async Task Delete_VoucherType()
            {
                // Arrange
                await ReseedTestDb.ReseedTestDbAsync(_context);
                DeleteVoucherTypeCommand command = new(3);
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




            private static CreateVoucherTypeCommand GetVoucherForCreate()
                => new()
                {
                    VoucherCode = 0,
                    VoucherType = "Test",
                    VoucherTitle = "Testing",
                    VoucherClassification = 1
                };

            private static UpdateVoucherTypeCommand GetVoucherForUpdate()
                => new()
                {
                    VoucherCode = 2,
                    VoucherType = "Test",
                    VoucherTitle = "Testing",
                    VoucherClassification = 1
                };
        }
    }
}