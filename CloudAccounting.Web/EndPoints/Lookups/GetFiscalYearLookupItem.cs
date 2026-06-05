using CloudAccounting.Application.UseCases.Lookups.GetFiscalYearLookupItem;
using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Web.EndPoints.Lookups;

public class GetFiscalYearLookupItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("lookups/fiscalyears/{companyCode:int}", GetFiscalYearsLookup)
            .Produces(401)
            .Produces(403)
            .Produces(404)
            .Produces<List<FiscalYearLookupItem>>()
            .Produces(500);
    }

    public static async Task<IResult> GetFiscalYearsLookup(
        int companyCode,
        ISender sender,
        ILogger<GetFiscalYearLookupItem> logger
    )
    {
        GetFiscalYearLookupItemQuery query = new(companyCode);
        Result<List<FiscalYearLookupItem>> result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning(
            "There was a problem getting fiscal years: {ERROR}", msg);
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