using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetVoucherTypeLookupItems;

public record GetVoucherTypeLookupItemsQuery : IQuery<List<VoucherTypeLookupItem>>;
