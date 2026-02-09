using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherType.GetVoucherTypes
{
    public record class GetAllVoucherTypesQuery() : IQuery<List<VoucherTypeDto>>;
}