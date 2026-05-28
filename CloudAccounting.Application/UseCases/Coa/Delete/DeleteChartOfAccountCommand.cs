namespace CloudAccounting.Application.UseCases.Coa.Delete;

public record DeleteChartOfAccountCommand(int CompanyCode, string AccountCode) : ICommand<MediatR.Unit>;