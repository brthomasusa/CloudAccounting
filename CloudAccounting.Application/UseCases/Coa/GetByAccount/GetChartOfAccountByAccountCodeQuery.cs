using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.GetByAccount;

public class GetChartOfAccountByAccountCodeQuery(int companyCode, string accountCode)
    : IQuery<ChartOfAccountDto>
{
    public int CompanyCode { get; } = companyCode;
    public string AccountCode { get; } = accountCode;
}