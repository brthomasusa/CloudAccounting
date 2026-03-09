using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Application.UseCases.FiscalYears.GetMostRecentFiscalYear
{
    public class GetMostRecentFiscalYearQueryHandler
    (
        IFiscalYearRepository repository,
        ILogger<GetMostRecentFiscalYearQueryHandler> logger
    ) : IQueryHandler<GetMostRecentFiscalYearQuery, FiscalYearDto>
    {
        private readonly IFiscalYearRepository _repository = repository;
        private readonly ILogger<GetMostRecentFiscalYearQueryHandler> _logger = logger;

        public async Task<Result<FiscalYearDto>> Handle
        (
            GetMostRecentFiscalYearQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<FiscalYear> result = await _repository.GetMostRecentFiscalYearAsync(query.CompanyCode);

                if (result.IsFailure)
                {
                    return Result<FiscalYearDto>.Failure<FiscalYearDto>(
                        new Error("GetMostRecentFiscalYearQueryHandler.Handle", result.Error.Message)
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
                    new Error("GetMostRecentFiscalYearQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}