using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.GetByAccount;

public class GetChartOfAccountByAccountCodeQueryHandler(
    IChartOfAccountRepository repository,
    ILogger<GetChartOfAccountByAccountCodeQueryHandler> logger
) : IQueryHandler<GetChartOfAccountByAccountCodeQuery, ChartOfAccountDto>
{
    public async Task<Result<ChartOfAccountDto>> Handle(GetChartOfAccountByAccountCodeQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.RetrieveAsync(query.CompanyCode, query.AccountCode);

            if (result.IsFailure)
                return Result.Failure<ChartOfAccountDto>(new Error("GetChartOfAccountByAccountCodeQueryHandler.Handle",
                    result.Error.Message));

            var coaDto = result.Value.Adapt<ChartOfAccountDto>();

            return Result.Success(coaDto);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<ChartOfAccountDto>(new Error("GetChartOfAccountByAccountCodeQueryHandler.Handle",
                errMsg));
        }
    }
}