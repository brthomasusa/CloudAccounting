namespace CloudAccounting.Core.Repositories
{
    public interface IVoucherTypeRepository
    {
        Task<Result<List<Voucher>>> RetrieveAllAsync();

        Task<Result<Voucher>> RetrieveAsync(int vchCode);

        Task<Result<Voucher>> CreateAsync(Voucher v);

        Task<Result<Voucher>> UpdateAsync(Voucher v);

        Task<Result> DeleteAsync(int id);

        Task<Result<bool>> IsUniqueVoucherTypeNameForCreate(string voucherTypeName);

        Task<Result<bool>> IsUniqueVoucherTypeNameForUpdate(int voucherTypeCode, string voucherTypeName);

        Task<Result<bool>> IsValidVoucherCode(int voucherCode);

        Task<Result<bool>> CanVoucherTypeBeDeleted(int voucherCode);
    }
}