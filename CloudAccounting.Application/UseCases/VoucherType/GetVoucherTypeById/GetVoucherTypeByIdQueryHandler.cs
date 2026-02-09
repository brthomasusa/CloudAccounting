using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypeById
{
    public class GetVoucherTypeByIdQueryHandler
    (
        IVoucherTypeReadRepository repository,
        ILogger<GetVoucherTypeByIdQueryHandler> logger,
        IMapper mapper
    ) : IQueryHandler<GetVoucherTypeByIdQuery, VoucherTypeDto>
    {
        private readonly IVoucherTypeReadRepository _repository = repository;
        private readonly ILogger<GetVoucherTypeByIdQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<VoucherTypeDto>> Handle
        (
            GetVoucherTypeByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<Voucher> result = await _repository.RetrieveAsync(query.VoucherCode);

                if (result.IsFailure)
                {
                    return Result<VoucherTypeDto>.Failure<VoucherTypeDto>(
                        new Error("GetVoucherTypeByIdQueryHandler.Handle", result.Error.Message)
                    );
                }

                VoucherTypeDto voucher = _mapper.Map<VoucherTypeDto>(result.Value);
                return voucher;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<VoucherTypeDto>.Failure<VoucherTypeDto>(
                    new Error("GetVoucherTypeByIdQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}