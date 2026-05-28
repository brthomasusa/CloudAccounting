using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.GetAll;

public record RetrieveAllQuery(
    int PageNumber,
    int PageSize,
    int CompanyCode
) : IQuery<PagedResponse<ChartOfAccountDto>>;