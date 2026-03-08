namespace CloudAccounting.Core.Services
{
    public interface IFiscalYearService
    {
        Task<Result<FiscalYear>> CreateFiscalYear(int companyCode, int fiscalYearNumber, DateTime startDate);

    }
}