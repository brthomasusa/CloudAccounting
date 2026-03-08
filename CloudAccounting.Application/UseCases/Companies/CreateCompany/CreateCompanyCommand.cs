using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Companies.CreateCompany;

public class CreateCompanyCommand : ICommand<CompanyDetailDto>
{
    public int CompanyCode { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? City { get; set; }

    public string? Zipcode { get; set; }

    public string? Currency { get; set; }
}
