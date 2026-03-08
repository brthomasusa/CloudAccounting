using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypeById
{
    public class GetVoucherTypeByIdQueryHandler
    (
        IVoucherTypeRepository repository,
        ILogger<GetVoucherTypeByIdQueryHandler> logger
    ) : IQueryHandler<GetVoucherTypeByIdQuery, VoucherTypeDto>
    {
        private readonly IVoucherTypeRepository _repository = repository;
        private readonly ILogger<GetVoucherTypeByIdQueryHandler> _logger = logger;

        public async Task<Result<VoucherTypeDto>> Handle
        (
            GetVoucherTypeByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<Voucher> getVoucherResult = await _repository.RetrieveAsync(query.VoucherCode);

                if (getVoucherResult.IsSuccess)
                {
                    return getVoucherResult.Value.Adapt<VoucherTypeDto>();
                }

                return Result<VoucherTypeDto>.Failure<VoucherTypeDto>(
                    new Error("GetVoucherTypeByIdQueryHandler.Handle", getVoucherResult.Error.Message)
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<VoucherTypeDto>.Failure<VoucherTypeDto>(
                    new Error("GetVoucherTypeByIdQueryHandler.Handle", errMsg)
                );
            }
        }
    }
}