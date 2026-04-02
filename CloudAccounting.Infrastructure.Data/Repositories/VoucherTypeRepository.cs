using CloudAccounting.Infrastructure.Data.Models;
using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Data;

namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public class VoucherTypeRepository
    (
        AppDbContext ctx,
        IMemoryCache memoryCache,
        ILogger<VoucherTypeRepository> logger
    ) : IVoucherTypeRepository
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

        private readonly AppDbContext _db = ctx;
        private readonly ILogger<VoucherTypeRepository> _logger = logger;

        public async Task<Result<List<Voucher>>> RetrieveAllAsync()
        {
            try
            {
                List<VoucherDM> dataModels = await _db.Vouchers.ToListAsync();

                List<Voucher> vouchers = [];
                dataModels.ForEach(dm =>
                {
                    vouchers.Add(dm.Adapt<Voucher>());
                });

                return vouchers;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<Voucher>>.Failure<List<Voucher>>(
                    new Error("VoucherRepository.RetrieveAllAsync", errMsg)
                );
            }
        }

        public async Task<Result<Voucher>> RetrieveAsync(int vchCode)
        {
            try
            {
                if (_memoryCache.TryGetValue($"voucher-{vchCode}", out VoucherDM? cachedVoucher))
                {
                    return cachedVoucher!.Adapt<Voucher>();
                }

                VoucherDM? dataModel = await _db.Vouchers.SingleOrDefaultAsync(v => v.VoucherCode == vchCode);

                if (dataModel != null)
                {
                    _memoryCache.Set($"voucher-{vchCode}", dataModel, _cacheEntryOptions);
                    return dataModel.Adapt<Voucher>();
                }

                return Result<Voucher>.Failure<Voucher>(
                    new Error("VoucherRepository.RetrieveAsync", $"No voucher found with code {vchCode}.")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<Voucher>.Failure<Voucher>(
                    new Error("VoucherRepository.RetrieveAsync", errMsg)
                );
            }
        }


        public async Task<Result<Voucher>> CreateAsync(Voucher v)
        {
            try
            {
                VoucherDM voucherDM = v.Adapt<VoucherDM>();

                EntityEntry<VoucherDM> added = await _db.Vouchers.AddAsync(voucherDM);

                int affected = await _db.SaveChangesAsync();

                if (affected == 1)
                {
                    _memoryCache.Set($"voucher-{v.VoucherCode}", voucherDM, _cacheEntryOptions);

                    return voucherDM.Adapt<Voucher>();
                }

                return Result<Voucher>.Failure<Voucher>(
                    new Error("VoucherRepository.CreateAsync", "Create voucher operation failed!")
                );
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<Voucher>.Failure<Voucher>(
                    new Error("VoucherRepository.CreateAsync", errMsg)
                );
            }
        }

        public async Task<Result<Voucher>> UpdateAsync(Voucher v)
        {
            try
            {
                await _db.Vouchers.Where(voucher => voucher.VoucherCode == v.VoucherCode)
                                   .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(voucher => voucher.VoucherType, v.VoucherType)
                                        .SetProperty(voucher => voucher.VoucherTitle, v.VoucherTitle)
                                        .SetProperty(voucher => voucher.VoucherClassification, v.VoucherClassification));

                VoucherDM? dataModel = await _db.Vouchers.FindAsync(v.VoucherCode);

                _memoryCache.Set($"voucher-{v.VoucherCode}", dataModel, _cacheEntryOptions);

                return dataModel.Adapt<Voucher>();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<Voucher>.Failure<Voucher>(
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

        public async Task<Result<bool>> IsUniqueVoucherTypeNameForCreate(string voucherTypeName)
        {
            try
            {
                bool exists = await _db.Vouchers.AnyAsync(v => v.VoucherType == voucherTypeName);

                return !exists;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("VoucherRepository.IsUniqueVoucherTypeNameForCreate", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsUniqueVoucherTypeNameForUpdate(int voucherTypeCode, string voucherTypeName)
        {
            try
            {
                bool exists = await _db.Vouchers
                                       .AnyAsync(v => v.VoucherType == voucherTypeName && v.VoucherCode != voucherTypeCode);

                return !exists;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("VoucherRepository.IsUniqueVoucherTypeNameForUpdate", errMsg)
                );
            }
        }

        public async Task<Result<bool>> IsValidVoucherCode(int voucherCode)
        {
            try
            {
                bool exists = await _db.Vouchers.AnyAsync(v => v.VoucherCode == voucherCode);

                return exists;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("VoucherRepository.IsExistingVoucherType", errMsg)
                );
            }
        }

        public async Task<Result<bool>> CanVoucherTypeBeDeleted(int voucherCode)
        {
            try
            {
                bool hasTransactions = await _db.TranMasters.AnyAsync(tm => tm.VoucherCode == voucherCode);

                return !hasTransactions;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<bool>.Failure<bool>(
                    new Error("VoucherRepository.CanVoucherTypeBeDeleted", errMsg)
                );
            }
        }
    }
}