
namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests.VoucherTypeTests
{
    public class VoucherTypeReadRepositoryTests : TestBase
    {
        private IVoucherTypeReadRepository _repository => new VoucherTypeReadRepository(_dapperContext!, new NullLogger<VoucherTypeReadRepository>());

        [Fact]
        public async Task RetrieveAllAsync_VoucherTypeReadRepository_ShouldReturn5Rows()
        {
            // Arrange


            // Act
            Result<List<Voucher>> result = await _repository.RetrieveAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task RetrieveAsync_VoucherTypeReadRepository_ShouldReturn1Row()
        {
            // Arrange
            int vchCode = 1;

            // Act
            Result<Voucher> result = await _repository.RetrieveAsync(vchCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("BPV", result.Value.VoucherType);
        }
    }
}