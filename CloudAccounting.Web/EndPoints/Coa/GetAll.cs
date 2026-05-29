using CloudAccounting.Application.UseCases.Coa.GetAll;
using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Web.EndPoints.Coa;

public class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("coa", GetAllCoaFromQuery)
            .Produces(404)
            .Produces<PagedResponse<ChartOfAccountDto>>()
            .Produces(500);
    }

    public static async Task<IResult> GetAllCoaFromQuery
    (
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] int companyCode,
        ISender sender,
        ILogger<GetAll> logger
    )
    {
        RetrieveAllQuery query = new(pageNumber, pageSize, companyCode);
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