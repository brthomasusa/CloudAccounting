namespace CloudAccounting.Core.Repositories
{
    public interface IVoucherTypeRepository
    {
        Task<Result<Voucher>> CreateAsync(Voucher v);

        Task<Result<Voucher>> UpdateAsync(Voucher v);

        Task<Result> DeleteAsync(int id);
    }
}