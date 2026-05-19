using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypes
{
    public record GetAllVoucherTypesQuery : IQuery<List<VoucherTypeDto>>;
}