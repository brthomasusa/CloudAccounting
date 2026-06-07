namespace CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserFiscalPeriod;

public record UpdateUserFiscalPeriodCommand(
    int CompanyCode,
    Int16 CompanyYear,
    byte CompanyMonthId
) : ICommand<MediatR.Unit>;