
namespace CloudAccounting.IntegrationTestsWebApi.VoucherTypeTests
{
    public class VoucherTypeRepositoryTests : TestBase
    {
        private readonly IVoucherTypeRepository _repo;
        private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

        public VoucherTypeRepositoryTests()
            => _repo = new VoucherTypeRepository(_efCoreContext!, _memoryCache!, new NullLogger<VoucherTypeRepository>(), _mapper);

        [Fact]
        public async Task CreateAsync_VoucherTypeRepository_CreatesAndReturnsOneVoucherType()
        {
            // Arrange
            Voucher voucher = GetVoucherForCreate();

            // Act
            Result<Voucher> result = await _repo.CreateAsync(voucher);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(voucher.VoucherType, result.Value.VoucherType);
        }

        [Fact]
        public async Task UpdateAsync_VoucherTypeRepository_UpdateAndReturnsOneVoucherType()
        {
            // Arrange
            Voucher voucher = GetVoucherForUpdate();

            // Act
            Result<Voucher> result = await _repo.UpdateAsync(voucher);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(voucher.VoucherType, result.Value.VoucherType);
        }

        [Fact]
        public async Task DeleteAsync_VoucherTypeRepository_DeleteOneVoucherType()
        {
            // Arrange
            int vchCode = 2;

            // Act
            Result result = await _repo.DeleteAsync(vchCode);
            var checkResult = await _efCoreContext!.Vouchers.FindAsync(vchCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(checkResult);
        }

        private static Voucher GetVoucherForCreate()
            => new()
            {
                VoucherCode = 0,
                VoucherType = "Test",
                VoucherTitle = "Testing",
                VoucherNature = 1
            };

        private static Voucher GetVoucherForUpdate()
            => new()
            {
                VoucherCode = 1,
                VoucherType = "Test",
                VoucherTitle = "Testing Update",
                VoucherNature = 1
            };
    }
}