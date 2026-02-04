namespace CloudAccounting.Application.UseCases.Lookups.CompanyCodeLookup
{
    public class CompanyCodeLookupQueryHandler
    (
        ICompanyReadRepository repository,
        ILogger<CompanyCodeLookupQueryHandler> logger
    ) : IQueryHandler<CompanyCodeLookupQuery, List<CompanyLookup>>
    {
        private readonly ICompanyReadRepository _repository = repository;
        private readonly ILogger<CompanyCodeLookupQueryHandler> _logger = logger;

        public async Task<Result<List<CompanyLookup>>> Handle
        (
        CompanyCodeLookupQuery query,
        CancellationToken cancellationToken
        )
        {
            try
            {
                Result<List<CompanyLookup>> result = await _repository.GetCompanyLookups();

                if (result.IsFailure)
                {
                    return Result<List<CompanyLookup>>.Failure<List<CompanyLookup>>(
                        new Error("CompanyCodeLookupQueryHandler.Handle", result.Error.Message)
                    );
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<List<CompanyLookup>>.Failure<List<CompanyLookup>>(
                    new Error("CompanyCodeLookupQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}