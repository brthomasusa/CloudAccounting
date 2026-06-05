using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetFiscalYearLookupItem;

public record GetFiscalYearLookupItemQuery(int CompanyCode) : IQuery<List<FiscalYearLookupItem>>;
