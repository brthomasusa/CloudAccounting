namespace CloudAccounting.IntegrationTests.VoucherTypeTests
{
    [Collection("SequentialTestCollection")]
    public class VoucherTypeRepositoryTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly AppDbContext _context = fixture.Context!;
        private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
        private IVoucherTypeRepository _repo => new VoucherTypeRepository(_context, _memoryCache!, new NullLogger<VoucherTypeRepository>());
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task RetrieveAllAsync_VoucherTypeRepository_ReturnsMultibleVoucherTypes()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            Result<List<Voucher>> result = await _repo.RetrieveAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task RetrieveAsync_VoucherTypeRepository_ShouldReturn1Row()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int vchCode = 1;

            // Act
            Result<Voucher> result = await _repo.RetrieveAsync(vchCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("BPV", result.Value.VoucherType);
        }

        [Fact]
        public async Task CreateAsync_VoucherTypeRepository_CreatesAndReturnsOneVoucherType()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
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
            await ReseedTestDb.ReseedTestDbAsync(_context);
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
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int vchCode = 2;

            // Act
            Result result = await _repo.DeleteAsync(vchCode);
            var checkResult = await _context!.Vouchers.FindAsync(vchCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(checkResult);
        }

        [Fact]
        public async Task IsUniqueVoucherTypeNameForCreate_VoucherTypeRepository_ShouldReturnTrue()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            string voucherTypeName = "Unique Voucher Type Name";

            // Act
            Result<bool> result = await _repo.IsUniqueVoucherTypeNameForCreate(voucherTypeName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsUniqueVoucherTypeNameForCreate_VoucherTypeRepository_ShouldReturnFalse()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            string voucherTypeName = "PO";

            // Act
            Result<bool> result = await _repo.IsUniqueVoucherTypeNameForCreate(voucherTypeName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task IsUniqueVoucherTypeNameForUpdate_VoucherTypeRepository_NewName_ShouldReturnTrue()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int voucherTypeCode = 1;
            string voucherTypeName = "Unique Voucher Type Name";

            // Act
            Result<bool> result = await _repo.IsUniqueVoucherTypeNameForUpdate(voucherTypeCode, voucherTypeName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsUniqueVoucherTypeNameForUpdate_VoucherTypeRepository_SameName_ShouldReturnTrue()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int voucherTypeCode = 1;
            string voucherTypeName = "BTechnical Consulting";

            // Act
            Result<bool> result = await _repo.IsUniqueVoucherTypeNameForUpdate(voucherTypeCode, voucherTypeName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsUniqueVoucherTypeNameForUpdate_VoucherTypeRepository_DuplicateName_ShouldReturnFalse()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int voucherTypeCode = 2;
            string voucherTypeName = "BPV";

            // Act
            Result<bool> result = await _repo.IsUniqueVoucherTypeNameForUpdate(voucherTypeCode, voucherTypeName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task IsExistingVoucherType_VoucherTypeRepository_ValidCode_ShouldReturnTrue()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int voucherTypeCode = 1;

            // Act
            Result<bool> result = await _repo.IsValidVoucherCode(voucherTypeCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsExistingVoucherType_VoucherTypeRepository_InvalidCode_ShouldReturnFalse()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int voucherTypeCode = 11;

            // Act
            Result<bool> result = await _repo.IsValidVoucherCode(voucherTypeCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task CanVoucherTypeBeDeleted_VoucherTypeRepository_ValidCode_ShouldReturnFalse()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int voucherTypeCode = 1;

            // Act
            Result<bool> result = await _repo.CanVoucherTypeBeDeleted(voucherTypeCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        private static Voucher GetVoucherForCreate()
            => new()
            {
                VoucherCode = 0,
                VoucherType = "Test",
                VoucherTitle = "Testing",
                VoucherClassification = 1
            };

        private static Voucher GetVoucherForUpdate()
            => new()
            {
                VoucherCode = 1,
                VoucherType = "Test",
                VoucherTitle = "Testing Update",
                VoucherClassification = 1
            };
    }
}