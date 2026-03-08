using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Companies.GetCompany;

public record GetCompanyByIdQuery(int CompanyCode) : IQuery<CompanyDetailDto>;
