using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.Update;

public record UpdateChartOfAccountCommand(
    int CompanyCode,
    string AccountCode,
    string AccountTitle,
    string AccountType,
    string CostCenterCode) : ICommand<ChartOfAccountDto>;
