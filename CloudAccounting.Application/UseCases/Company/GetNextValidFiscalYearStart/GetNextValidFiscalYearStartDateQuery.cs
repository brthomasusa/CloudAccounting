namespace CloudAccounting.Application.UseCases.Company.GetNextValidFiscalYearStart
{
    public record GetNextValidFiscalYearStartDateQuery(int CompanyCode)
        : IQuery<DateTime>;
}