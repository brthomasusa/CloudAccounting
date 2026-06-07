using CloudAccounting.Application.UseCases.IdentityManagement.UpdateUserFiscalPeriod;

namespace CloudAccounting.Web.EndPoints.FiscalYears;

public class UpdateUserFiscalPeriod : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("fiscalyears/", UpdateUserFiscalPeriodInfo)
            .Produces(204)
            .Produces(500);
    }

    // [Authorize(Roles = "CompanyAdmin")]
    public static async Task<IResult> UpdateUserFiscalPeriodInfo
    (
        [FromBody] UpdateUserFiscalPeriodCommand command,
        ISender sender,
        ILogger<UpdateUserFiscalPeriod> logger
    )
    {
        Result<Unit> result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.NoContent();
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