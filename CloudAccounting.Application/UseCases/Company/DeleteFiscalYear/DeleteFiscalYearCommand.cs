namespace CloudAccounting.Application.UseCases.Company.DeleteFiscalYear
{
    public record class DeleteFiscalYearCommand(int CompanyCode, int FiscalYear) : ICommand<MediatR.Unit>;
}