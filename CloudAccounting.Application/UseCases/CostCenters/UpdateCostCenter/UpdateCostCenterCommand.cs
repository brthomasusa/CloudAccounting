
namespace CloudAccounting.Application.UseCases.CostCenters.UpdateCostCenter;

public record UpdateCostCenterCommand(
    int CompanyCode,
    string CostCenterCode,
    string CostCenterTitle
) : ICommand<CostCenterDto>;