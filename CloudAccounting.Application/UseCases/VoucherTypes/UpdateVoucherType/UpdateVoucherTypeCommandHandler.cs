using CloudAccounting.Infrastructure.Data.Models;
using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType
{
    public class UpdateVoucherTypeCommandHandler(
        IVoucherTypeRepository repository,
        ILogger<UpdateVoucherTypeCommandHandler> logger
    ) : ICommandHandler<UpdateVoucherTypeCommand, VoucherTypeDto>
    {
        private readonly IVoucherTypeRepository _repository = repository;
        private readonly ILogger<UpdateVoucherTypeCommandHandler> _logger = logger;

        public async Task<Result<VoucherTypeDto>> Handle(UpdateVoucherTypeCommand command, CancellationToken token)
        {
            Voucher voucher = command.Adapt<Voucher>();
            Result<Voucher> result = await _repository.UpdateAsync(voucher);

            if (result.IsFailure)
            {
                return Result<VoucherTypeDto>.Failure<VoucherTypeDto>(
                    new Error("UpdateVoucherTypeCommandHandler.Handle", result.Error.Message)
                );
            }

            VoucherTypeDto voucherTypeDto = result.Value.Adapt<VoucherTypeDto>();

            return voucherTypeDto;
        }
    }
}