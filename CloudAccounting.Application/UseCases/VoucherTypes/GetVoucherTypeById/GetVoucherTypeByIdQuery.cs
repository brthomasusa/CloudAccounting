using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypeById
{
    public record class GetVoucherTypeByIdQuery(int VoucherCode) : IQuery<VoucherTypeDto>;
}