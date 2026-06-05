using CloudAccounting.Application.UseCases.Lookups.GetFiscalPeriodLookupItems;
using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Web.EndPoints.Lookups;

public class GetFiscalPeriodLookupItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("lookups/fiscalperiods/{companyCode:int}/{companyYear:int}", GetFiscalPeriodsLookup)
            .Produces(401)
            .Produces(403)
            .Produces(404)
            .Produces<List<FiscalPeriodLookupItem>>()
            .Produces(500);
    }

    public static async Task<IResult> GetFiscalPeriodsLookup(
        int companyCode,
        int companyYear,
        ISender sender,
        ILogger<GetFiscalPeriodLookupItem> logger
    )
    {
        GetFiscalPeriodLookupQuery query = new(companyCode, companyYear);
        Result<List<FiscalPeriodLookupItem>> result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning(
            "There was a problem getting fiscal periods: {ERROR}", msg);
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