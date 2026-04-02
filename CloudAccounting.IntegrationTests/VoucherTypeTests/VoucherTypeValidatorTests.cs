using CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType;

namespace CloudAccounting.IntegrationTests.VoucherTypeTests
{
    [Collection("SequentialTestCollection")]
    public class VoucherTypeValidatorTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly AppDbContext _context = fixture.Context!;
        private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
        private IVoucherTypeRepository _repo => new VoucherTypeRepository(_context, _memoryCache!, new NullLogger<VoucherTypeRepository>());
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_ShouldHaveNoValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_DuplicateVoucherType_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherType = "LSI"; // Assuming "LSI" already exists in the seeded test database, this should trigger the uniqueness validation error.

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherType)
                .WithErrorMessage("This voucher type already exists.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_NullVoucherType_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherType = null!;

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherType)
                .WithErrorMessage("Missing voucher type.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_EmptyVoucherType_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherType = string.Empty;

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherType)
                .WithErrorMessage("Missing voucher type.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_VoucherTypeLengthExceeded_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherType = new string('A', 7); // 7 characters, exceeding the 6-character limit

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherType)
                .WithErrorMessage("Voucher type cannot exceed 6 characters.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_VoucherTitleLengthExceeded_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherTitle = new string('A', 31); // 31 characters, exceeding the 30-character limit

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherTitle)
                .WithErrorMessage("Voucher title cannot exceed 30 characters.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_VoucherTitleNull_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherTitle = null!;

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherTitle)
                .WithErrorMessage("Missing voucher title.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_VoucherTitleEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherTitle = string.Empty;

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherTitle)
                .WithErrorMessage("Missing voucher title.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_VoucherClassificationTooHigh_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherClassification = 11; // Exceeds the maximum allowed value

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherClassification)
                .WithErrorMessage("Voucher classification must be between 1 and 3.");
        }

        [Fact]
        public async Task CreateVoucherTypeCommandValidator_VoucherClassificationTooLow_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandValidator validator = new(_repo);
            CreateVoucherTypeCommand command = GetVoucherForCreate();
            command.VoucherClassification = 0; // Below the minimum allowed value

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherClassification)
                .WithErrorMessage("Voucher classification must be between 1 and 3.");
        }

        [Fact]
        public async Task UpdateVoucherTypeCommandValidator_ShouldHaveNoValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            UpdateVoucherTypeCommandValidator validator = new(_repo);
            UpdateVoucherTypeCommand command = GetVoucherForUpdate();

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task UpdateVoucherTypeCommandValidator_UpdateDoesNotChangeVoucherType_ShouldHaveNoValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            UpdateVoucherTypeCommandValidator validator = new(_repo);
            UpdateVoucherTypeCommand command = GetVoucherForUpdate();
            command.VoucherType = "LSI"; // Same as existing voucher type, should not trigger uniqueness validation

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task UpdateVoucherTypeCommandValidator_DuplicateVoucherType_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            UpdateVoucherTypeCommandValidator validator = new(_repo);
            UpdateVoucherTypeCommand command = GetVoucherForUpdate();
            command.VoucherType = "BRV"; // Assuming "BRV" already exists in the seeded test database, this should trigger the uniqueness validation error.

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherType)
                .WithErrorMessage("This voucher type already exists.");
        }

        [Fact]
        public async Task DeleteVoucherTypeCommandValidator_ShouldHaveNoValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            DeleteVoucherTypeCommandValidator validator = new(_repo);
            DeleteVoucherTypeCommand command = new(2); // Assuming voucher code 2 exists in the seeded test database

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task DeleteVoucherTypeCommandValidator_InvalidVoucherCode_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            DeleteVoucherTypeCommandValidator validator = new(_repo);
            DeleteVoucherTypeCommand command = new(0); // Invalid voucher code


            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherCode)
                .WithErrorMessage("Missing voucher code.");
        }

        [Fact]
        public async Task DeleteVoucherTypeCommandValidator_VoucherCodeDoesNotExist_ShouldHaveValidationErrors()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            DeleteVoucherTypeCommandValidator validator = new(_repo);
            DeleteVoucherTypeCommand command = new(25); // Invalid voucher code


            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherCode)
                .WithErrorMessage("The voucher code is not valid.");
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