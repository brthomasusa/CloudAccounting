using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Application.UseCases.FiscalYears.GetFiscalYear;

public class GetFiscalYearQueryHandler
(
    IFiscalYearRepository repository,
    ILogger<GetFiscalYearQueryHandler> logger
) : IQueryHandler<GetFiscalYearQuery, FiscalYearDto>
{
    private readonly IFiscalYearRepository _repository = repository;
    private readonly ILogger<GetFiscalYearQueryHandler> _logger = logger;

    public async Task<Result<FiscalYearDto>> Handle
    (
        GetFiscalYearQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Result<FiscalYear> result = await _repository.GetFiscalYearAsync(query.CompanyCode, query.FiscalYearNumber);

            if (result.IsFailure)
            {
                return Result<FiscalYearDto>.Failure<FiscalYearDto>(
                    new Error("GetFiscalYearQueryHandler.Handle", result.Error.Message)
                );
            }

            FiscalYearDto fiscalYear = result.Value.Adapt<FiscalYearDto>();

            return fiscalYear;
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            _logger.LogError(ex, "{Message}", errMsg);

            return Result<FiscalYearDto>.Failure<FiscalYearDto>(
                new Error("GetFiscalYearQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}
