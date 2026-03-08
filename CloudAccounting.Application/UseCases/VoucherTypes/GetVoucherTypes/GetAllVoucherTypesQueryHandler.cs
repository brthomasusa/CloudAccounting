using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypes
{
    public class GetAllVoucherTypesQueryHandler
    (
        IVoucherTypeRepository repository,
        ILogger<GetAllVoucherTypesQueryHandler> logger
    ) : IQueryHandler<GetAllVoucherTypesQuery, List<VoucherTypeDto>>
    {
        private readonly IVoucherTypeRepository _repository = repository;
        private readonly ILogger<GetAllVoucherTypesQueryHandler> _logger = logger;

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
                    List<VoucherTypeDto> voucherDtos = [];
                    getAllVoucherResult.Value.ForEach(voucher =>
                    {
                        voucherDtos.Add(voucher.Adapt<VoucherTypeDto>());       //TODO: Consider using Mapster's ProjectToType for better performance if the list is large and the mapping is straightforward.
                    });

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