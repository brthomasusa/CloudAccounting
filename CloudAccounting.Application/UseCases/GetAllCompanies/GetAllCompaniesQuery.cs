using CloudAccounting.Application.ViewModels.Company;

namespace CloudAccounting.Application.UseCases.GetAllCompanies;

public record GetAllCompaniesQuery(int PageNumber, int PageSize) : IQuery<List<CompanyDetailVm>>;
