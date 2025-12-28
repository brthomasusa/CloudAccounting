namespace CloudAccounting.Core.Exceptions;

public class CompanyNotFoundException : Exception
{
    public CompanyNotFoundException(int companyCode)
        : base($"The company with CompanyCode '{companyCode}' was not found.")
    {
        CompanyCode = companyCode;
    }

    public int CompanyCode { get; }
}