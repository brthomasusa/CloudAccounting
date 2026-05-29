using CloudAccounting.Application.UseCases.Coa.Delete;

namespace CloudAccounting.Web.EndPoints.Coa;

public class DeleteCoa : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("coa/{companyCode:int}/{accountCode}", DeleteChartOfAccount)
            .Produces(204)
            .Produces(400)
            .Produces(500);
    }

    public static async Task<IResult> DeleteChartOfAccount(int companyCode, string accountCode,
        ISender sender,
        ILogger<DeleteCoa> logger)
    {
        var command = new DeleteChartOfAccountCommand(companyCode, accountCode);
        var result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem deleting the account: {ERROR}", msg);
        return Results.BadRequest(msg);
    }
}