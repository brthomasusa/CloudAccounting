using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Company.GetCompanyWithNoFiscalYearData
{
    // This can return a company (like a new company) that has no fiscal year info

    public class GetCompanyWithNoFiscalYearDataQueryHandler
    (
        ICompanyReadRepository repository,
        ILogger<GetCompanyWithNoFiscalYearDataQueryHandler> logger
    ) : IQueryHandler<GetCompanyWithNoFiscalYearDataQuery, CompanyWithFiscalPeriodsDto>
    {
        private readonly ICompanyReadRepository _repository = repository;
        private readonly ILogger<GetCompanyWithNoFiscalYearDataQueryHandler> _logger = logger;

        public async Task<Result<CompanyWithFiscalPeriodsDto>> Handle
        (
            GetCompanyWithNoFiscalYearDataQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<CompanyWithFiscalPeriodsDto> result = await _repository.GetLatestFiscalYearForCompanyAsync(query.CompanyCode);

                if (result.IsFailure)
                {
                    return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                        new Error("GetCompanyWithNoFiscalYearDataQueryHandler.Handle", result.Error.Message)
                    );
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                    new Error("GetCompanyWithNoFiscalYearDataQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}