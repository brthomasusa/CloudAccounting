namespace CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType
{
    public class DeleteVoucherTypeCommandHandler
    (
        IVoucherTypeRepository repository,
        ILogger<DeleteVoucherTypeCommandHandler> logger
    ) : ICommandHandler<DeleteVoucherTypeCommand, MediatR.Unit>
    {
        private readonly IVoucherTypeRepository _repository = repository;
        private readonly ILogger<DeleteVoucherTypeCommandHandler> _logger = logger;

        public async Task<Result<MediatR.Unit>> Handle(DeleteVoucherTypeCommand command, CancellationToken token)
        {
            Result result = await _repository.DeleteAsync(command.VoucherCode);

            if (result.IsFailure)
            {
                return Result<MediatR.Unit>.Failure<MediatR.Unit>(
                    new Error("DeleteVoucherTypeCommandHandler.Handle", result.Error.Message)
                );
            }

            return MediatR.Unit.Value;
        }
    }
}
