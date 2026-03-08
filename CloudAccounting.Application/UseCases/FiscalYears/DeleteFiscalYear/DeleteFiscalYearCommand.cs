namespace CloudAccounting.Application.UseCases.FiscalYears.DeleteFiscalYear;

public record class DeleteFiscalYearCommand(int CompanyCode, int FiscalYearNumber) : ICommand<MediatR.Unit>;

