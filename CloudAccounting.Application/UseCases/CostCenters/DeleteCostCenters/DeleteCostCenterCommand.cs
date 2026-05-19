namespace CloudAccounting.Application.UseCases.CostCenters.DeleteCostCenters;

public record DeleteCostCenterCommand(
    int CompanyCode,
    string CostCenterCode
) : ICommand<MediatR.Unit>;