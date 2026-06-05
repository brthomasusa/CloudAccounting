using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Application.UseCases.Lookups.GetFiscalPeriodLookupItems;

public class GetFiscalPeriodLookupQueryHandler(
    ILookupRepository lookupRepository,
    ILogger<GetFiscalPeriodLookupQueryHandler> logger
) : IQueryHandler<GetFiscalPeriodLookupQuery, List<FiscalPeriodLookupItem>>
{
    public async Task<Result<List<FiscalPeriodLookupItem>>> Handle
    (
        GetFiscalPeriodLookupQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await lookupRepository.RetrieveFiscalPeriodsAsync(query.CompanyCode, query.CompanyYear);

            if (!result.IsSuccess)
                return Result.Failure<List<FiscalPeriodLookupItem>>(
                    new Error("GetFiscalPeriodLookupQueryHandler.Handle", result.Error.Message)
                );

            return Result.Success(result.Value);
        }
        catch (Exception ex)
        {
            string errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<List<FiscalPeriodLookupItem>>(
                new Error("GetFiscalPeriodLookupQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
            );
        }
    }
}