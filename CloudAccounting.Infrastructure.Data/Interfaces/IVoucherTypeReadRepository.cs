using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Interfaces
{
    public interface IVoucherTypeReadRepository
    {
        Task<Result<List<Voucher>>> RetrieveAllAsync();

        Task<Result<Voucher>> RetrieveAsync(int vchCode);
    }
}