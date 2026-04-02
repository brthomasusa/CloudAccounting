using CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypeById;
using CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypes;
using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.IntegrationTests.VoucherTypeTests
{
    [Collection("SequentialTestCollection")]
    public class VoucherTypeQueryHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly AppDbContext _context = fixture.Context!;
        private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
        private IVoucherTypeRepository _repo => new VoucherTypeRepository(_context, _memoryCache!, new NullLogger<VoucherTypeRepository>());
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task GetVoucherTypeByIdQueryHandler_ShouldReturnVoucherType()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            GetVoucherTypeByIdQuery query = new GetVoucherTypeByIdQuery(1);
            GetVoucherTypeByIdQueryHandler handler = new(_repo, new NullLogger<GetVoucherTypeByIdQueryHandler>());

            // Act
            Result<VoucherTypeDto> result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("BPV", result.Value?.VoucherType);
        }

        [Fact]
        public async Task GetAllVoucherTypesQueryHandler_ShouldReturnMultibleVoucherTypes()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            GetAllVoucherTypesQuery query = new();
            GetAllVoucherTypesQueryHandler handler = new(_repo, new NullLogger<GetAllVoucherTypesQueryHandler>());

            // Act
            Result<List<VoucherTypeDto>> result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }



    }
}