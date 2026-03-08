using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetCompanyLookupItems
{
    public record class GetCompanyLookupItemsQuery() : IQuery<List<CompanyLookupItem>>;
}