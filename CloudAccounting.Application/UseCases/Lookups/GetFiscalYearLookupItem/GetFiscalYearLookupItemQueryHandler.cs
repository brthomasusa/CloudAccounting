
using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetFiscalYearLookupItem;

public class GetFiscalYearLookupItemQueryHandler(
    ILookupRepository lookupRepository,
    ILogger<GetFiscalYearLookupItemQueryHandler> logger
) : IQueryHandler<GetFiscalYearLookupItemQuery, List<FiscalYearLookupItem>>
{
    public async Task<Result<List<FiscalYearLookupItem>>> Handle
    (
        GetFiscalYearLookupItemQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await lookupRepository.RetrieveFiscalYearsAsync(query.CompanyCode);

            if (!result.IsSuccess)
                return Result.Failure<List<FiscalYearLookupItem>>(
                    new Error("GetFiscalYearLookupItemQueryHandler.Handle", result.Error.Message)
                );

            return Result.Success(result.Value);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<FiscalYearLookupItem>>(
                new Error("GetFiscalYearLookupItemQueryHandler.Handle", errMsg)
            );
        }
    }
}