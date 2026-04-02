using CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType;
using CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType;
using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.IntegrationTests.VoucherTypeTests
{
    [Collection("SequentialTestCollection")]
    public class VoucherTypeCommandHandlerTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly AppDbContext _context = fixture.Context!;
        private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
        private IVoucherTypeRepository _repo => new VoucherTypeRepository(_context, _memoryCache!, new NullLogger<VoucherTypeRepository>());
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task Handle_CreateVoucherTypeCommandHandler_GivenValidCmd_ShouldSucceed()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            CreateVoucherTypeCommandHandler handler = new(_repo, new NullLogger<CreateVoucherTypeCommandHandler>());
            CreateVoucherTypeCommand command = GetVoucherForCreate();

            // Act
            Result<VoucherTypeDto> result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.VoucherCode > 2);
            Assert.Equal(command.VoucherType, result.Value.VoucherType);
        }

        [Fact]
        public async Task Handle_UpdateVoucherTypeCommandHandler_GivenValidCmd_ShouldSucceed()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            UpdateVoucherTypeCommandHandler handler = new(_repo, new NullLogger<UpdateVoucherTypeCommandHandler>());
            UpdateVoucherTypeCommand command = GetVoucherForUpdate();

            // Act
            Result<VoucherTypeDto> result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(command.VoucherType, result.Value.VoucherType);
        }

        [Fact]
        public async Task Handle_DeleteVoucherTypeCommandHandler_GivenValidCmd_ShouldSucceed()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            DeleteVoucherTypeCommandHandler handler = new(_repo, new NullLogger<DeleteVoucherTypeCommandHandler>());
            DeleteVoucherTypeCommand command = new(2);

            // Act
            Result<MediatR.Unit> result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
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