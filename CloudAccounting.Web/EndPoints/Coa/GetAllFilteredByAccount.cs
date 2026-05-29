using CloudAccounting.Application.UseCases.Coa.GetFilterByAccount;
using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Web.EndPoints.Coa;

public class GetAllFilteredByAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("coa/filtered", GetAllCoaFilteredFromQuery)
            .Produces(404)
            .Produces<PagedResponse<ChartOfAccountDto>>()
            .Produces(500);
    }

    public static async Task<IResult> GetAllCoaFilteredFromQuery
    (
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] int companyCode,
        [FromQuery] string accountCode,
        ISender sender,
        ILogger<GetAllFilteredByAccount> logger
    )
    {
        RetrieveAllByAccountQuery query = new(pageNumber, pageSize, companyCode, accountCode);
        var result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem getting the chart of accounts: {ERROR}", msg);
        return Results.NotFound(msg);
    }
}