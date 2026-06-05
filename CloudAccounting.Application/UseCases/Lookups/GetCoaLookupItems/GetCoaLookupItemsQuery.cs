using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetCoaLookupItems;

public record GetCoaLookupItemsQuery(int CompanyCode) : IQuery<List<CoaLookupItem>>;