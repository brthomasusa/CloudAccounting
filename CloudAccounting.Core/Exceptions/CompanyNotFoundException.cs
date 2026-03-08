namespace CloudAccounting.Core.Exceptions;

public class CompanyNotFoundException(int companyCode) : Exception($"The company with CompanyCode '{companyCode}' was not found.")
{

    public int CompanyCode { get; } = companyCode;
}