using CloudAccounting.Shared;

namespace CloudAccounting.Application.UseCases.CostCenters.CreateCostCenter;

public record CreateCostCenterCommand
(
    int CompanyCode, 
    string CostCenterCode, 
    string CostCenterTitle
) : ICommand<CostCenterDto>;