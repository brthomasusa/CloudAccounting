namespace CloudAccounting.Core.Exceptions
{
    public class FiscalYearDeleteException(int companyCode, int fiscalYearNumber)
        : InvalidOperationException($"The company with CompanyCode '{companyCode}' has transactions for year {fiscalYearNumber} and thus can't be deleted.")
    {
        public int CompanyCode { get; } = companyCode;
        public int FiscalYearNumber { get; } = fiscalYearNumber;
    }
}