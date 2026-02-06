namespace CloudAccounting.Application.UseCases.Company.GetNextValidFiscalYearStart
{
    public class GetNextValidFiscalYearStartDateQueryHandler
    (
        ICompanyReadRepository repository,
        ILogger<GetNextValidFiscalYearStartDateQueryHandler> logger
    ) : IQueryHandler<GetNextValidFiscalYearStartDateQuery, DateTime>
    {
        private readonly ICompanyReadRepository _repository = repository;
        private readonly ILogger<GetNextValidFiscalYearStartDateQueryHandler> _logger = logger;

        public async Task<Result<DateTime>> Handle
        (
            GetNextValidFiscalYearStartDateQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<DateTime> result = await _repository.GetNextValidFiscalYearStartDate(query.CompanyCode);

                if (result.IsFailure)
                {
                    return Result<DateTime>.Failure<DateTime>(
                        new Error("GetNextValidFiscalYearStartDateQueryHandler.Handle", result.Error.Message)
                    );
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<DateTime>.Failure<DateTime>(
                    new Error("GetNextValidFiscalYearStartDateQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}