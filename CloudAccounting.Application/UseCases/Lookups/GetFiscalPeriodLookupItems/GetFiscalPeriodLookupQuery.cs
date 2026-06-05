using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetFiscalPeriodLookupItems;

public record GetFiscalPeriodLookupQuery(int CompanyCode, int CompanyYear) : IQuery<List<FiscalPeriodLookupItem>>;
