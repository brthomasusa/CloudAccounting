using CloudAccounting.Application.UseCases.Lookups.GetVoucherTypeLookupItems;
using CloudAccounting.Shared.Lookups;

namespace CloudAccounting.Web.EndPoints.Lookups;

public class GetVoucherTypeLookupItems : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("lookups/vouchertypes", GetVoucherTypeLookup)
            .Produces(401)
            .Produces(403)
            .Produces(404)
            .Produces<List<VoucherTypeLookupItem>>() //.RequireAuthorization()
            .Produces(500);
    }

    public static async Task<IResult> GetVoucherTypeLookup(
        ISender sender,
        ILogger<GetVoucherTypeLookupItems> logger
    )
    {
        GetVoucherTypeLookupItemsQuery query = new();
        Result<List<VoucherTypeLookupItem>> result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        string msg = result.Error.Message;
        logger.LogWarning(
            "There was a problem getting voucher type lookup items: {ERROR}", msg);
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