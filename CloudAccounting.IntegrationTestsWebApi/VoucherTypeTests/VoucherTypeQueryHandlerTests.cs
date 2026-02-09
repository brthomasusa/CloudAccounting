using CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypes;
using CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypeById;
using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.IntegrationTestsWebApi.VoucherTypeTests
{
    public class VoucherTypeQueryHandlerTests : TestBase
    {
        private readonly VoucherTypeReadRepository _readRepository;
        private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

        public VoucherTypeQueryHandlerTests()
            => _readRepository = new(_dapperContext!, new NullLogger<VoucherTypeReadRepository>());

        [Fact]
        public async Task Handle_GetVoucherTypeByIdQueryHandler_ShouldReturn_1_VoucherTypeDto()
        {
            // Arrange
            GetVoucherTypeByIdQuery query = new(1);
            GetVoucherTypeByIdQueryHandler handler = new(_readRepository, new NullLogger<GetVoucherTypeByIdQueryHandler>(), _mapper);

            // Act
            Result<VoucherTypeDto> result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("BPV", result.Value.VoucherType);
        }

        [Fact]
        public async Task Handle_GetAllVoucherTypesQueryHandler_ShouldReturn_5_VoucherTypeDtos()
        {
            // Arrange
            GetAllVoucherTypesQuery query = new();
            GetAllVoucherTypesQueryHandler handler = new(_readRepository, new NullLogger<GetAllVoucherTypesQueryHandler>(), _mapper);

            // Act
            Result<List<VoucherTypeDto>> result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }
    }
}