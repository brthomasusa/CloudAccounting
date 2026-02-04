namespace CloudAccounting.Application.UseCases.Lookups.CompanyCodeLookup
{
    public record class CompanyCodeLookupQuery() : IQuery<List<CompanyLookup>>;

}