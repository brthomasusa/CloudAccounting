namespace CloudAccounting.Application.Models
{
    public record CompanyDto
    (
        int CompanyCode,
        string CompanyName,
        string Address,
        string City,
        string Zipcode,
        string Phone,
        string Fax,
        string Currency,
        List<FiscalYearDto> FiscalYears
    );
}