using CloudAccounting.Core.Models;
using CloudAccounting.Infrastructure.Data.Interfaces;

namespace CloudAccounting.Infrastructure.Data.Repositories.Read
{
    public class VoucherTypeReadRepository
    (
        DapperContext dapperContext,
        ILogger<VoucherTypeReadRepository> logger
    ) : IVoucherTypeReadRepository
    {
        private readonly DapperContext _dapperContext = dapperContext;
        private ILogger<VoucherTypeReadRepository> _logger = logger;

        public async Task<Result<List<Voucher>>> RetrieveAllAsync()
        {
            try
            {
                string sql =
                    @"SELECT 
                        vchcode,
                        vchtype,
                        vchtitle,
                        vchnature
                    FROM gl_voucher 
                    ORDER BY vchtype";

                using var connection = _dapperContext.CreateConnection();

                var vouchers = await connection.QueryAsync<Voucher>(sql);

                return vouchers.ToList();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<Voucher>>.Failure<List<Voucher>>(
                    new Error("VoucherReadRepository.RetrieveAllAsync", errMsg)
                );
            }
        }

        public async Task<Result<Voucher>> RetrieveAsync(int vchCode)
        {
            try
            {
                string sql =
                    @"SELECT 
                        vchcode AS VoucherCode,
                        vchtype AS VoucherType,
                        vchtitle AS VoucherTitle,
                        vchnature AS VoucherNature
                    FROM gl_voucher 
                    WHERE VCHCODE = :VCHCODE";

                var param = new { VCHCODE = vchCode };

                using var connection = _dapperContext.CreateConnection();

                Voucher voucher = await connection.QuerySingleAsync<Voucher>(sql, param).ConfigureAwait(false);

                return voucher;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<Voucher>.Failure<Voucher>(
                    new Error("VoucherReadRepository.RetrieveAsync", errMsg)
                );
            }
        }
    }
}