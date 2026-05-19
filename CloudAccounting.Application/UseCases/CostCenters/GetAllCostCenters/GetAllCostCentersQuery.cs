

namespace CloudAccounting.Application.UseCases.CostCenters.GetAllCostCenters;

public record GetAllCostCentersQuery(int CompanyCode) : IQuery<List<CostCenterDto>>;
