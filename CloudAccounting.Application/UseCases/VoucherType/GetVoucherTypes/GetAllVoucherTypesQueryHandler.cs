using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypes
{
    public class GetAllVoucherTypesQueryHandler
    (
        IVoucherTypeReadRepository repository,
        ILogger<GetAllVoucherTypesQueryHandler> logger,
        IMapper mapper
    ) : IQueryHandler<GetAllVoucherTypesQuery, List<VoucherTypeDto>>
    {
        private readonly IVoucherTypeReadRepository _repository = repository;
        private readonly ILogger<GetAllVoucherTypesQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<VoucherTypeDto>>> Handle
        (
            GetAllVoucherTypesQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<List<Voucher>> getAllVoucherResult = await _repository.RetrieveAllAsync();

                if (getAllVoucherResult.IsSuccess)
                {
                    List<VoucherTypeDto> voucherDtos = _mapper.Map<List<VoucherTypeDto>>(getAllVoucherResult.Value);

                    return voucherDtos;
                }

                return Result<List<VoucherTypeDto>>.Failure<List<VoucherTypeDto>>(
                    new Error("GetAllVoucherTypesQueryHandler.Handle", getAllVoucherResult.Error.Message)
                );

            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<VoucherTypeDto>>.Failure<List<VoucherTypeDto>>(
                    new Error("GetAllVoucherTypesQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}