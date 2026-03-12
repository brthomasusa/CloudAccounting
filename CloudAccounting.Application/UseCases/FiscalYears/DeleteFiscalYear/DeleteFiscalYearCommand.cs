namespace CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;

public record class DeleteFiscalYearCommand(int CompanyCode, int FiscalYear) : ICommand<MediatR.Unit>;

