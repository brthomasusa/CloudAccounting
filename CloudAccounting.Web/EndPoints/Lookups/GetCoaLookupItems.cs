using CloudAccounting.Application.UseCases.Lookups.GetCoaLookupItems;
using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Web.EndPoints.Lookups;

public class GetCoaLookupItems : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("lookups/coa/{companyCode:int}", GetCoaLookup)
            .Produces(401)
            .Produces(403)
            .Produces(404)
            .Produces<List<CoaLookupItem>>()
            .Produces(500);
    }

    public static async Task<IResult> GetCoaLookup(
        int companyCode,
        ISender sender,
        ILogger<GetCoaLookupItems> logger
    )
    {
        GetCoaLookupItemsQuery query = new(companyCode);
        Result<List<CoaLookupItem>> result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning(
            "There was a problem getting the chart of accounts: {ERROR}", msg);
        return Results.Problem(
            detail: msg,
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Internal Server Error",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { msg } }
            });
    }
}