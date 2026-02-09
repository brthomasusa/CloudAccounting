using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypeById
{
    public record class GetVoucherTypeByIdQuery(int VoucherCode) : IQuery<VoucherTypeDto>;
}