using CloudAccounting.Application.UseCases.CostCenters.UpdateCostCenter;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Web.EndPoints.CostCenters;

public class UpdateCostCenter : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("costcenters", UpdateCostCenterFromBody)
            .Produces(201)
            .Produces(400)
            .Produces(500);
    }

    public static async Task<IResult> UpdateCostCenterFromBody(UpdateCostCenterCommand command,
        ISender sender,
        ILogger<UpdateCostCenter> logger)
    {
        var result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem updating the cost center: {ERROR}", msg);
        return Results.BadRequest(msg);
    }
}