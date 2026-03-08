using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Companies.GetAllCompanies;

public record GetAllCompaniesQuery(int PageNumber, int PageSize) : IQuery<List<CompanyDetailDto>>;
