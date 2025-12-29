using CloudAccounting.Application.ViewModels.Company;
namespace CloudAccounting.Application.UseCases.CreateCompany;

public class CreateCompanyCommand : ICommand<CompanyDetailVm>
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
