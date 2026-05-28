using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.GetFilterByAccount;

public record RetrieveAllByAccountQuery(
    int PageNumber,
    int PageSize,
    int CompanyCode,
    string AccountCode
) : IQuery<PagedResponse<ChartOfAccountDto>>;