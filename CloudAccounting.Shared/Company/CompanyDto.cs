namespace CloudAccounting.Shared.Company
{
    public record class CompanyDto
    (
        int CompanyCode,
        string CompanyName,
        string Address,
        string Phone,
        string Fax,
        string City,
        string Zipcode,
        string Currency
    );
}