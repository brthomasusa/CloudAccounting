using CloudAccounting.Application.UseCases.CostCenters.DeleteCostCenters;

namespace CloudAccounting.Web.EndPoints.CostCenters;

public class DeleteCostCenter : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("costcenters/{companyCode:int}/{costCenterCode}", DeleteCostCenterFromBody)
            .Produces(204)
            .Produces(400)
            .Produces(500);
    }

    public static async Task<IResult> DeleteCostCenterFromBody(int companyCode, string costCenterCode,
        ISender sender,
        ILogger<DeleteCostCenter> logger)
    {
        var command = new DeleteCostCenterCommand(companyCode, costCenterCode);
        var result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem deleting the cost center: {ERROR}", msg);
        return Results.BadRequest(msg);
    }
}