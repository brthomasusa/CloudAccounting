using CloudAccounting.Application.UseCases.CostCenters.CreateCostCenter;
using CloudAccounting.Shared;

namespace CloudAccounting.Web.EndPoints.CostCenters;

public class CreateCostCenter : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("costcenters", CreateCostCenterFromBody)
            .Produces(201)
            .Produces(400)
            .Produces(500);
    }

    // [Authorize(Roles = "CompanyAdmin")]
    public static async Task<IResult> CreateCostCenterFromBody(CreateCostCenterCommand command,
        ISender sender,
        ILogger<CreateCostCenter> logger)
    {
        var result = await sender.Send(command);

        if (result.IsSuccess)
        {
            return Results.Created($"costcenters/{command.CompanyCode}/{command.CostCenterCode}", result.Value);
        }

        var msg = result.Error.Message;
        logger.LogWarning("There was a problem creating the cost center: {ERROR}", msg);
        return Results.BadRequest(msg);
    }
}
