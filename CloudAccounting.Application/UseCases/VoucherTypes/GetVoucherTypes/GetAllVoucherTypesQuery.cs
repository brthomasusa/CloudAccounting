using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.GetVoucherTypes
{
    public record class GetAllVoucherTypesQuery() : IQuery<List<VoucherTypeDto>>;
}