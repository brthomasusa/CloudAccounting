namespace CloudAccounting.Application.UseCases.CostCenters.GetCostCenter;

public record GetCostCenterQuery
(
    int CompanyCode, 
    string CostCenterCode
) : IQuery<CostCenterDto>;