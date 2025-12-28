using CloudAccounting.Application.ViewModels.Company;

namespace CloudAccounting.Application.UseCases.GetCompanyById;

public record GetCompanyByIdQuery(int CompanyCode) : IQuery<CompanyDetailVm>;
