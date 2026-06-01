using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetCostCenterLookupItems;

public record GetCostCenterLookupItemQuery(int CompanyCode) : IQuery<List<CostCenterLookupItem>>;

