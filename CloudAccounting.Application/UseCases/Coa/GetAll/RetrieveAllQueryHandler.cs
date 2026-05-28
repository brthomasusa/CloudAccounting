using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.UseCases.Coa.GetAll;

public class RetrieveAllQueryHandler(
    IChartOfAccountRepository repository,
    ILogger<RetrieveAllQueryHandler> logger
) : IQueryHandler<RetrieveAllQuery, PagedResponse<ChartOfAccountDto>>
{
    public async Task<Result<PagedResponse<ChartOfAccountDto>>> Handle(RetrieveAllQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.RetrieveAllAsync(query.PageNumber, query.PageSize, query.CompanyCode);

            if (result.IsFailure)
                return Result.Failure<PagedResponse<ChartOfAccountDto>>(new Error("RetrieveAllQueryHandler.Handle",
                    result.Error.Message));

            var coaDtos = result.Value.Data.Adapt<List<ChartOfAccountDto>>();
            var pagedResponse = new PagedResponse<ChartOfAccountDto>(coaDtos, result.Value.PageNumber,
                result.Value.PageSize, result.Value.TotalRecords);

            return Result.Success(pagedResponse);
        }
        catch (Exception ex)
        {
            var errMsg = Helpers.GetInnerExceptionMessage(ex);
            logger.LogError(ex, "{Message}", errMsg);

            return Result.Failure<PagedResponse<ChartOfAccountDto>>(new Error("RetrieveAllQueryHandler.Handle",
                errMsg));
        }
    }
}