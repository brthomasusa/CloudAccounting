namespace CloudAccounting.Core.Exceptions
{
    public class CompanyDeleteException(int companyCode)
        : InvalidOperationException($"The company with CompanyCode '{companyCode}' has fiscal year info attached and thus can't be deleted.")
    {
        public int CompanyCode { get; } = companyCode;
    }
}