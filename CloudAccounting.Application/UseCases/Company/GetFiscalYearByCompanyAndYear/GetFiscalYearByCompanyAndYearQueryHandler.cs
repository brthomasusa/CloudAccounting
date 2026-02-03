using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Company.GetFiscalYearByCompanyAndYear
{
    public class GetFiscalYearByCompanyAndYearQueryHandler
    (
    ICompanyReadRepository repository,
    ILogger<GetFiscalYearByCompanyAndYearQueryHandler> logger
    ) : IQueryHandler<GetFiscalYearByCompanyAndYearQuery, CompanyWithFiscalPeriodsDto>
    {
        private readonly ICompanyReadRepository _repository = repository;
        private readonly ILogger<GetFiscalYearByCompanyAndYearQueryHandler> _logger = logger;

        public async Task<Result<CompanyWithFiscalPeriodsDto>> Handle
        (
            GetFiscalYearByCompanyAndYearQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<CompanyWithFiscalPeriodsDto> result = await _repository.GetFiscalYearDtoByIdAndYearAsync(query.CompanyCode, query.FiscalYear);

                if (result.IsFailure)
                {
                    return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                        new Error("GetFiscalYearByCompanyAndYearQueryHandler.Handle", result.Error.Message)
                    );
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                    new Error("GetFiscalYearByCompanyAndYearQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}