namespace CloudAccounting.Infrastructure.Data.Repositories
{
    public interface ILookupRepository
    {
        Task<Result<List<CompanyLookupItem>>> RetrieveAllAsync();

        Task<Result<List<CostCenterLookupItem>>> RetrieveCostCentersAsync(int companyCode);

        Task<Result<List<FiscalYearLookupItem>>> RetrieveFiscalYearsAsync(int companyCode);

        Task<Result<List<FiscalPeriodLookupItem>>> RetrieveFiscalPeriodsAsync(int companyCode, int companyYear);

        Task<Result<List<VoucherTypeLookupItem>>> RetrieveVoucherTypesAsync();

        Task<Result<List<CoaLookupItem>>> RetrieveLedgerAccountsAsync(int companyCode);
    }
}