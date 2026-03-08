using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType
{
    public class CreateVoucherTypeCommandHandler
    (
        IVoucherTypeRepository repo,
        ILogger<CreateVoucherTypeCommandHandler> logger
    ) : ICommandHandler<CreateVoucherTypeCommand, VoucherTypeDto>
    {
        private readonly IVoucherTypeRepository _repo = repo;
        private readonly ILogger<CreateVoucherTypeCommandHandler> _logger = logger;

        public async Task<Result<VoucherTypeDto>> Handle(CreateVoucherTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Voucher voucher = request.Adapt<Voucher>();

                Result<Voucher> result = await _repo.CreateAsync(voucher);

                if (result.IsFailure)
                {
                    string errMsg = result.Error.Message;
                    _logger.LogError("{Message}", errMsg);
                    return Result<VoucherTypeDto>.Failure<VoucherTypeDto>(new Error("CreateVoucherTypeCommandHandler.Handle", errMsg));
                }

                return result.Value.Adapt<VoucherTypeDto>();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);
                return Result<VoucherTypeDto>.Failure<VoucherTypeDto>(new Error("CreateVoucherTypeCommandHandler.Handle", errMsg));
            }
        }
    }
}