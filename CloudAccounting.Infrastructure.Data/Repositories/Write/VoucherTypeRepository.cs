using VoucherDataModel = CloudAccounting.Infrastructure.Data.Models.Voucher;
using VoucherDomainModel = CloudAccounting.Core.Models.Voucher;

namespace CloudAccounting.Infrastructure.Data.Repositories.Write
{
    public class VoucherTypeRepository
    (
        CloudAccountingContext ctx,
        IMemoryCache memoryCache,
        ILogger<VoucherTypeRepository> logger,
        IMapper mapper
    ) : IVoucherTypeRepository
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

        private readonly CloudAccountingContext _db = ctx;
        private readonly ILogger<VoucherTypeRepository> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<VoucherDomainModel>> CreateAsync(VoucherDomainModel v)
        {
            try
            {
                VoucherDataModel voucherDataModel = _mapper.Map<VoucherDataModel>(v);

                EntityEntry<VoucherDataModel> added = await _db.Vouchers.AddAsync(voucherDataModel);

                int affected = await _db.SaveChangesAsync();

                if (affected == 1)
                {
                    _memoryCache.Set($"voucher-{v.VoucherCode}", voucherDataModel, _cacheEntryOptions);

                    return _mapper.Map<VoucherDomainModel>(voucherDataModel);
                }

                return Result<VoucherDomainModel>.Failure<VoucherDomainModel>(
                    new Error("VoucherRepository.CreateAsync", "Create voucher operation failed!")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<VoucherDomainModel>.Failure<VoucherDomainModel>(
                    new Error("VoucherRepository.CreateAsync", errMsg)
                );
            }
        }

        public async Task<Result<VoucherDomainModel>> UpdateAsync(VoucherDomainModel v)
        {
            try
            {
                await _db.Vouchers.Where(voucher => voucher.VoucherCode == v.VoucherCode)
                                   .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(voucher => voucher.VoucherType, v.VoucherType)
                                        .SetProperty(voucher => voucher.VoucherTitle, v.VoucherTitle)
                                        .SetProperty(voucher => voucher.VoucherClassification, v.VoucherClassification));

                VoucherDataModel? dataModel = await _db.Vouchers.FindAsync(v.VoucherCode);

                _memoryCache.Set($"voucher-{v.VoucherCode}", dataModel, _cacheEntryOptions);

                return _mapper.Map<VoucherDomainModel>(dataModel!);
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<VoucherDomainModel>.Failure<VoucherDomainModel>(
                    new Error("VoucherRepository.UpdateAsync", errMsg)
                );
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                await _db.Vouchers.Where(v => v.VoucherCode == id)
                                  .ExecuteDeleteAsync();

                _memoryCache.Remove($"voucher-{id}");

                return Result.Success();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result.Failure(new Error("VoucherRepository.DeleteAsync", errMsg));
            }
        }
    }
}